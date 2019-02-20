﻿using System;
using Medical_Examiner_API.Loggers;
using Medical_Examiner_API.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Medical_Examiner_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IMELogger, MELogger>();

            services.AddScoped<ControllerActionFilter>();

            services.AddScoped<IExaminationPersistence>(s =>
            {
                return new ExaminationPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"],
                    Configuration["CosmosDB:DatabaseId"]);
            });

            services.AddScoped<IUserPersistence>(s =>
            {
                return new UserPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"],
                    Configuration["CosmosDB:DatabaseId"]);
            });


            services.AddScoped<IMeLoggerPersistence>(s =>
            {
                return new MeLoggerPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"],
                    Configuration["CosmosDB:DatabaseId"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });


            app.UseHttpsRedirection();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}