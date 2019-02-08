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
using static Mapper.AlertMapper;
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

            builder.RegisterType<AlertService>()
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

            builder.RegisterGeneric(typeof(CreateCommandHandler<>))
                .As(typeof(IRequestHandler<,>))
                .InstancePerLifetimeScope();

            builder
              .RegisterType<DBNotificationHandler>()
              .As<INotificationHandler<LoggerEvent>>()
              .InstancePerLifetimeScope();

            builder
             .RegisterType<FileNotificationHandler>()
             .As<INotificationHandler<LoggerEvent>>()
             .InstancePerLifetimeScope();

            //register your profiles, or skip this if you don't want them in your container
            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(UserMapper))
                .As<Profile>();

            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(NotesMapper))
                .As<Profile>();

            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(AlertMapper))
                .As<Profile>();

            builder.RegisterAssemblyTypes()
                .AssignableTo(typeof(OrderMapper))
                .As<Profile>();

            builder.RegisterType<UserResolver>()
               .AsSelf();

            //register your configuration as a single instance
            builder.Register(container => {
                var context = container.Resolve<IComponentContext>();

                var confg = new MapperConfiguration(mc => {
                    //add your profiles (either resolve from container or however else you acquire them)
                    mc.AddProfile(new UserMapper());
                    mc.AddProfile(new NotesMapper());
                    mc.AddProfile(new AlertMapper());
                    mc.AddProfile(new OrderMapper());
                    mc.ConstructServicesUsing(context.Resolve);
                });
                confg.AssertConfigurationIsValid();
                return confg;
            })
            .AsSelf()
            .As<IConfigurationProvider>()
            .SingleInstance();

            //register mapper
            //builder.Register(c => c.Resolve<MapperConfiguration>()
            //.CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            builder.Register(c => {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .SingleInstance();

            builder.Register(c => {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<UserContext>();
                opt.UseSqlServer(config.GetSection("ConnectionDB").Value);

                return new UserContext(opt.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            //call main class load
            base.Load(builder);
        }
    }
}