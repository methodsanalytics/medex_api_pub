using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using AutoMapper;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Services;
using MedicalExaminer.API.Services.Implementations;
using MedicalExaminer.Common;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.MedicalTeam;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MedicalExaminer.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add services to the container.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var oktaSettingsSection = Configuration.GetSection("Okta");
            var okatSettings = oktaSettingsSection.Get<OktaSettings>();
            services.Configure<OktaSettings>(oktaSettingsSection);

            ConfigureOktaClient(services);

            services.AddSingleton<ITokenService, OktaTokenService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            ConfigureAuthentication(services, okatSettings);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            Mapper.Initialize(config =>
            {
                config.AddMedicalExaminerProfiles();
            });

            services.AddAutoMapper();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Medical Examiner Data API", Version = "v1" });

                // Make all enums appear as strings
                c.DescribeAllEnumsAsStrings();

                // Locate the XML file being generated by ASP.NET.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);

                // Make swagger do authentication
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Array.Empty<string>() },
                };

                c.AddSecurityDefinition("Okta", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = "https://***REMOVED***.oktapreview.com/oauth2/default/v1/authorize",
                    Scopes = new Dictionary<string, string>()
                    {
                        { "profile", "Profile" },
                        { "openid", "OpenID" },
                    },
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
            });

            services.AddScoped<IMELogger, MELogger>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();
            services.AddScoped<ILocationConnectionSettings>(s => new LocationConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IUserConnectionSettings>(s => new UserConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IExaminationConnectionSettings>(s => new ExaminationConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<ExaminationQueryBuilder>(s => new ExaminationQueryBuilder());
            services
                .AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>,
                    ExaminationsDashboardService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>, UserRetrievalByIdService>();

            services.AddScoped<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>, UserRetrievalByEmailService>();
            services.AddScoped<IAsyncQueryHandler<CreateExaminationQuery, Examination>, CreateExaminationService>();
            services.AddScoped<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>, ExaminationRetrievalService>();
            services.AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>, ExaminationsRetrievalService>();
            services.AddScoped<IAsyncUpdateDocumentHandler,  MedicalTeamUpdateService>();
            services.AddScoped<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>, PatientDetailsUpdateService>();
            services.AddScoped<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>, PatientDetailsRetrievalService>();

            services.AddScoped<IAsyncQueryHandler<CreateUserQuery, MeUser>, CreateUserService>();


            services.AddScoped<ControllerActionFilter>();

            services.AddScoped<ILocationPersistence>(s => new LocationPersistence(
                 new Uri(Configuration["CosmosDB:URL"]),
                 Configuration["CosmosDB:PrimaryKey"],
                 Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IUserPersistence>(s => new UserPersistence(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IMeLoggerPersistence>(s => new MeLoggerPersistence(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IPermissionPersistence>(s => new PermissionPersistence(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The App.</param>
        /// <param name="env">The Environment.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            // TODO: Not using HTTPS while we join front to back end
            //app.UseHttpsRedirection();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    // Use a bespoke Index that has OpenID/CustomJS to handle OKTA
                    c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MedicalExaminer.API.SwaggerIndex.html");

                    var oktaSettings = app.ApplicationServices.GetRequiredService<IOptions<OktaSettings>>();

                    c.OAuthConfigObject = new OAuthConfigObject()
                    {
                        ClientId = oktaSettings.Value.ClientId,
                        ClientSecret = oktaSettings.Value.ClientSecret,
                    };
                    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { {"nonce",  Guid.NewGuid().ToString() } });

                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            // Must be above UseMvc
            app.UseAuthentication();

            app.UseCors("MyPolicy");

            app.UseMvc();
        }

        /// <summary>
        /// Configure basic authentication so we can use tokens.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="oktaSettings">Okta Settings.</param>
        private void ConfigureAuthentication(IServiceCollection services, OktaSettings oktaSettings)
        {
            var provider = services.BuildServiceProvider();
            var tokenService = provider.GetRequiredService<ITokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = oktaSettings.Authority;
                    options.Audience = oktaSettings.Audience;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(
                        new OktaJwtSecurityTokenHandler(
                            tokenService,
                            new JwtSecurityTokenHandler()));
                });
        }

        /// <summary>
        /// Configure Okta Client.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureOktaClient(IServiceCollection services)
        {
            // Configure okta client
            services.AddScoped<OktaClientConfiguration>(context =>
            {
                var settings = context.GetRequiredService<IOptions<OktaSettings>>();
                return new OktaClientConfiguration
                {
                    OktaDomain = settings.Value.Domain,
                    Token = settings.Value.SdkToken,
                };
            });
            services.AddScoped<OktaClient, OktaClient>();
        }
    }
}