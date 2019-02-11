﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API;
using Medical_Examiners_API.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Medical_Examiners_API
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


            services.AddLogging();


            services.AddScoped<ControllerActionFilter>();
           

            services.AddScoped<IExaminationPersistence>((s) =>
            {
                return new ExaminationPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"]);
            });

            services.AddScoped<IUserPersistence>((s) =>
            {
                return new UserPersistence(
                    new Uri(Configuration["CosmosDB:URL"]),
                    Configuration["CosmosDB:PrimaryKey"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddConsole(LogLevel.Information);
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
