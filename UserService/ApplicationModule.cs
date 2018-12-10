using Autofac;
using AutoMapper;
using Common;
using Common.Commands;
using Common.Interface;
using Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository;
using Service;
using System.Collections.Generic;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;
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

            builder.RegisterType<GetAsyncHandler<UserModel>>()
                .As<IRequestHandler<GetCommandAsync<UserModel>, UserModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DeleteAsyncHandler<UserModel>>()
               .As<IRequestHandler<DeleteAsyncCommand<UserModel>, bool>>()
               .InstancePerLifetimeScope();

            //register your profiles, or skip this if you don't want them in your container
            builder.RegisterAssemblyTypes().AssignableTo(typeof(UserMapper)).As<Profile>();
            builder.RegisterAssemblyTypes().AssignableTo(typeof(NotesMapper)).As<Profile>();

            //register your configuration as a single instance
            builder.Register(c => {
                var confg = new MapperConfiguration(mc => {
                    //add your profiles (either resolve from container or however else you acquire them)
                    //foreach (var profile in c.Resolve<IEnumerable<Profile>>()) {
                    //    mc.AddProfile(profile);
                    //}
                    mc.AddProfile(new UserMapper());
                    mc.AddProfile(new NotesMapper());
                });
                confg.AssertConfigurationIsValid();
                return confg;
            }).AsSelf()
            .As<IConfigurationProvider>()
            .SingleInstance();

            //register your mapper
            builder.Register(c => c.Resolve<MapperConfiguration>()
            .CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            builder.Register(c => {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<UserContext>();
                opt.UseSqlServer(config.GetSection("ConnectionDB").Value);

                return new UserContext(opt.Options);
            }).AsSelf().InstancePerLifetimeScope();

            //call main class load
            base.Load(builder);
        }
    }
}