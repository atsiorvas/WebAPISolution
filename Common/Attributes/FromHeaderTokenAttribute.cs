using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Linq;

namespace Common.Attributes {
    [AttributeUsage(
        AttributeTargets.Property
        | AttributeTargets.Parameter,
        AllowMultiple = false,
        Inherited = true)]
    public class FromHeaderTokenAttribute
        : Attribute, IBindingSourceMetadata, IModelNameProvider {

        public BindingSource BindingSource { get; }

        public string Name { get; set; }

        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Metadata.IsCollectionType &&
                (context.Metadata.ModelType.GetTypeInfo().IsInterface ||
                 context.Metadata.ModelType.GetTypeInfo().IsAbstract) &&
                (context.BindingInfo.BindingSource == null ||
                !context.BindingInfo.BindingSource
                .CanAcceptDataFrom(BindingSource.Services))) {

                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();

                context.Metadata.Properties.ToList().ForEach((property) => {
                    propertyBinders.Add(property, context.CreateBinder(property));
                });

                return new InterfacesModelBinder(propertyBinders);
            }

            return null;
        }
    }

    public class InterfacesModelBinder : ComplexTypeModelBinder {

        public InterfacesModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinder)
            : base(propertyBinder) {

        }
        protected override object CreateModel(ModelBindingContext bindingContext) {
            return bindingContext.HttpContext
                .RequestServices.GetService(bindingContext.ModelType);
        }
    }
}