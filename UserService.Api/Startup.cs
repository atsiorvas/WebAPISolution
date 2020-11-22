using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.AbstractValidator;
using Common.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;

namespace UserService {
    public class Startup {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services) {

            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserAppService, UserAppService>();
            //services.AddScoped<INoteService, NoteService>();
            //services.AddScoped(typeof(UnitOfWork));
            services.AddHttpContextAccessor();
            //services.TryAddSingleton<HttpContext, HttpContext>();
            services.TryAddSingleton(new HttpClient());

            // Auto Mapper Configurations
            //var mappingConfig = new MapperConfiguration(mc => {
            //    mc.AllowNullDestinationValues = true;
            //    mc.CreateMissingTypeMaps = false;
            //    mc.AllowNullCollections = true;
            //    mc.AddProfile(new UserMapper());
            //    mc.AddProfile(new NotesMapper());
            //    mc.AddProfile(new AlertMapper());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            //services.AddAutoMapper(Assembly.GetAssembly(typeof(UserMapper)));

            //add health check for this service
            //services.AddHealthChecks(checks => {
            //    var minutes = 1;

            //    if (int.TryParse(_configuration["HealthCheck:Timeout"],
            //        out var minutesParsed)) {
            //        minutes = minutesParsed;
            //    }
            //    //checks.AddSqlCheck("UsersContext", _configuration["ConnectionDB"]);
            //});

            services.AddIdentityServer()
                 // .AddInMemoryIdentityResources(Config.GetIdentityResources())
                 .AddInMemoryApiResources(Config.GetApis())
                 .AddInMemoryClients(Config.GetClients());

            // rest omitted

            //services.AddDbContext<UserContext>(options =>
            //   options.UseSqlServer(_configuration["ConnectionDB"]));

            //services.AddMediatR(Assembly.GetAssembly(typeof(GetUserAsyncCommand)));
            //services.AddMediatR(Assembly.GetAssembly(typeof(CreateCommand<UserModel>)));

            services.AddCors();
            services.AddMvc()
                .AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UserValidation>())
                .AddJsonOptions(options => {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //options.SerializerSettings.Converters.Add(new UserConverter());
                });

            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            //container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env) {

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                //app.UseHsts();
            }

            app.UseCors(x =>
                x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // This Middleware accept every http request from ip contains into
            // appsettings.json
            app.UseMiddleware<AdminSafeListMiddleware>(
               _configuration["AdminSafeList"]);

            // app.UseHttpsRedirection();
            //app.UseIdentityServer();

            // Middleware to return exception error to response
            app.UseMiddleware<EndpointMiddlewareErrorTrapper>();
            app.UseMvc();
        }
    }
}