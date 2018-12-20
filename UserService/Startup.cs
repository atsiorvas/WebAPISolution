﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repository;
using System;
using System.IO;
using System.Text;
using UserService.Controllers;

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

            //// Auto Mapper Configurations
            //var mappingConfig = new MapperConfiguration(mc => {
            //    mc.AllowNullDestinationValues = true;
            //    mc.CreateMissingTypeMaps = false;
            //    mc.AllowNullCollections = true;
            //    mc.AddProfile(new UserMapper());
            //    mc.AddProfile(new NotesMapper());
            //});

            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            //services.AddAutoMapper(Assembly.GetAssembly(typeof(UserMapper)));

            //add health check for this service
            services.AddHealthChecks(checks => {
                var minutes = 1;

                if (int.TryParse(_configuration["HealthCheck:Timeout"],
                    out var minutesParsed)) {
                    minutes = minutesParsed;
                }
                //checks.AddSqlCheck("UsersContext", _configuration["ConnectionDB"]);
            });

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
            }
            app.UseCors(x => {
                x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
            //app.Use(async (context, next) => {
            //    var bodyStr = "";
            //    var req = context.Request;

            //    // Allows using several time the stream in ASP.Net Core
            //    req.EnableRewind();

            //    // Arguments: Stream, Encoding, detect encoding, buffer size 
            //    // AND, the most important: keep stream opened
            //    using (StreamReader reader
            //              = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true)) {
            //        bodyStr = reader.ReadToEnd();
            //    }

            //    // Rewind, so the core is not lost when it looks the body for the request
            //    req.Body.Position = 0;

            //    await next.Invoke();
            //});
            app.UseMiddleware<EndpointMiddlewareErrorTrapper>();
            app.UseMvc();
        }
    }
}