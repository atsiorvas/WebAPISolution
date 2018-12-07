using Autofac;
using AutoMapper;
using Common;
using Common.Commands;
using Common.Interface;
using Mapper;
using MediatR;
using Repository;
using Service;
using System.Collections.Generic;
using Module = Autofac.Module;

namespace UserService {
    public class ApplicationModule : Module {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr) {
            QueriesConnectionString = qconstr;

        }
        public ApplicationModule() { }

        protected override void Load(ContainerBuilder builder) {

            builder
                  .RegisterType<Mediator>()
                  .As<IMediator>()
                  .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context => {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserAppService>()
                .As<IUserAppService>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PreserveExistingDefaults();

            builder.RegisterType<NoteService>()
                .As<INoteService>()
                .AsSelf()
               .InstancePerLifetimeScope();

            builder.RegisterType(typeof(UnitOfWork))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreateCommandHandler<UserModel>>()
                .As<IRequestHandler<CreateCommand<UserModel>, UserModel>>()
                .InstancePerLifetimeScope();

            //register your profiles, or skip this if you don't want them in your container
            builder.RegisterAssemblyTypes().AssignableTo(typeof(UserMapper)).As<Profile>();
            builder.RegisterAssemblyTypes().AssignableTo(typeof(NotesMapper)).As<Profile>();

            //register your configuration as a single instance
            builder.Register(c => {
                var confg = new MapperConfiguration(mc => {
                    //add your profiles (either resolve from container or however else you acquire them)
                    foreach (var profile in c.Resolve<IEnumerable<Profile>>()) {
                        mc.AddProfile(profile);
                    }
                });
                confg.AssertConfigurationIsValid();
                return confg;
            }).AsSelf()
            .As<IConfigurationProvider>()
            .SingleInstance();

            //register your mapper
            builder.Register(c => c.Resolve<MapperConfiguration>()
            .CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

        }
    }
}