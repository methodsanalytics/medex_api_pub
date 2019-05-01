using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using AutoMapper;
using MedicalExaminer.API.Authorization;
using MedicalExaminer.API.Extensions.Data;
using MedicalExaminer.API.Filters;
using MedicalExaminer.API.Models;
using MedicalExaminer.API.Services;
using MedicalExaminer.API.Services.Implementations;
using MedicalExaminer.Common;
using MedicalExaminer.Common.Authorization;
using MedicalExaminer.Common.ConnectionSettings;
using MedicalExaminer.Common.Database;
using MedicalExaminer.Common.Loggers;
using MedicalExaminer.Common.Queries.CaseBreakdown;
using MedicalExaminer.Common.Queries.Examination;
using MedicalExaminer.Common.Queries.Location;
using MedicalExaminer.Common.Queries.PatientDetails;
using MedicalExaminer.Common.Queries.User;
using MedicalExaminer.Common.Services;
using MedicalExaminer.Common.Services.Examination;
using MedicalExaminer.Common.Services.Location;
using MedicalExaminer.Common.Services.MedicalTeam;
using MedicalExaminer.Common.Services.PatientDetails;
using MedicalExaminer.Common.Services.User;
using MedicalExaminer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Okta.Sdk;
using Okta.Sdk.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

using Cosmonaut.Extensions;
using Cosmonaut.Configuration;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Cosmonaut;
using MedicalExaminer.Common.Queries.CaseOutcome;
using MedicalExaminer.Common.Services.CaseOutcome;

namespace MedicalExaminer.API
{
    /// <summary>
    ///     Startup
    /// </summary>
    public class Startup
    {
        private const string ApiTitle = "Medical Examiner API";
        private const string ApiDescription = "The API for the Medical Examiner Service.";

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
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

            ConfigureAuthorization(services);

            services.AddMvcCore()
                .AddVersionedApiExplorer(options =>
                {
                    // The format of the version added to the route URL
                    options.GroupNameFormat = "'v'VVV";

                    // Tells swagger to replace the version in the controller route
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddApiVersioning(config => { config.ReportApiVersions = true; });

            Mapper.Initialize(config => { config.AddMedicalExaminerProfiles(); });
            //Mapper.AssertConfigurationIsValid();
            services.AddAutoMapper();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // c.SwaggerDoc("v1", new Info { Title = "Medical Examiner Data API", Version = "v1" });

                // note: need a temporary service provider here because one has not been created yet
                var provider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, ApiTitle, ApiDescription));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // Make all enums appear as strings
                options.DescribeAllEnumsAsStrings();

                // Locate the XML file being generated by ASP.NET.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Swagger to use those XML comments.
                options.IncludeXmlComments(xmlPath);

                // Make swagger do authentication
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Array.Empty<string>() },
                };

                options.AddSecurityDefinition("Okta", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = okatSettings.AuthorizationUrl,
                    Scopes = new Dictionary<string, string>
                    {
                        { "profile", "Profile" },
                        { "openid", "OpenID" },
                    },
                });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(security);
            });

            // Logger 
            services.AddScoped<IMELogger, MELogger>();
            
            // Database connections  
            services.AddScoped<IDocumentClientFactory, DocumentClientFactory>();
            services.AddScoped<IDatabaseAccess, DatabaseAccess>();

            services.AddScoped<ControllerActionFilter>();

            var cosmosSettings = new CosmosStoreSettings(
                Configuration["CosmosDB:DatabaseId"],
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"]);

            services.AddCosmosStore<Examination>(cosmosSettings, "Examinations");

            ConfigureQueries(services);

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
        ///     Configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The App.</param>
        /// <param name="env">The Environment.</param>
        /// <param name="loggerFactory">The Logger Factory.</param>
        /// <param name="provider">API Version Description Privider.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
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

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    // Use a bespoke Index that has OpenID/CustomJS to handle OKTA
                    c.IndexStream = () =>
                        GetType().GetTypeInfo().Assembly
                            .GetManifestResourceStream("MedicalExaminer.API.SwaggerIndex.html");

                    var oktaSettings = app.ApplicationServices.GetRequiredService<IOptions<OktaSettings>>();

                    c.OAuthConfigObject = new OAuthConfigObject
                    {
                        ClientId = oktaSettings.Value.ClientId,
                        ClientSecret = oktaSettings.Value.ClientSecret
                    };
                    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                        { { "nonce", Guid.NewGuid().ToString() } });

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                    // c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            // Must be above UseMvc
            app.UseAuthentication();

            app.UseCors("MyPolicy");

            app.UseMvc();
        }

        /// <summary>
        /// Configure Queries.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureQueries(IServiceCollection services)
        {
            services.AddScoped<ILocationConnectionSettings>(s => new LocationConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IExaminationConnectionSettings>(s => new ExaminationConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            services.AddScoped<IUserConnectionSettings>(s => new UserConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]));

            // Examination services
            services.AddScoped(s => new ExaminationsQueryExpressionBuilder());
            services.AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, ExaminationsOverview>,ExaminationsDashboardService>();
            services.AddScoped<IAsyncQueryHandler<CreateExaminationQuery, Examination>, CreateExaminationService>();
            services.AddScoped<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>, ExaminationRetrievalService>();
            services.AddScoped<IAsyncQueryHandler<ExaminationsRetrievalQuery, IEnumerable<Examination>>, ExaminationsRetrievalService>();
            services.AddScoped<IAsyncQueryHandler<ExaminationRetrievalQuery, Examination>, ExaminationRetrievalService>();
            services.AddScoped<IAsyncQueryHandler<CreateEventQuery, string>, CreateEventService>();

            // Medical team services
            services.AddScoped<IAsyncUpdateDocumentHandler, MedicalTeamUpdateService>();

            // Case Outcome Confirmation of Scrutiny
            services.AddScoped<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>, ConfirmationOfScrutinyService>();

            // Patient details services
            services.AddScoped<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>, PatientDetailsUpdateService>();
            services.AddScoped<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>, PatientDetailsRetrievalService>();

            // Case Outcome Services
            services.AddScoped<IAsyncQueryHandler<CloseCaseQuery, string>, CloseCaseService>();
            services.AddScoped<IAsyncQueryHandler<CoronerReferralQuery, string>, CoronerReferralService>();
            services.AddScoped<IAsyncQueryHandler<SaveOutstandingCaseItemsQuery, string>, SaveOutstandingCaseItemsService>();
            services.AddScoped<IAsyncQueryHandler<ConfirmationOfScrutinyQuery, Examination>, ConfirmationOfScrutinyService>();

            // Patient details services 
            services.AddScoped<IAsyncQueryHandler<PatientDetailsUpdateQuery, Examination>, PatientDetailsUpdateService>();
            services.AddScoped<IAsyncQueryHandler<PatientDetailsByCaseIdQuery, Examination>, PatientDetailsRetrievalService>();

            // User services 
            services.AddScoped<IAsyncQueryHandler<CreateUserQuery, MeUser>, CreateUserService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByEmailQuery, MeUser>, UserRetrievalByEmailService>();
            services.AddScoped<IAsyncQueryHandler<UserRetrievalByIdQuery, MeUser>, UserRetrievalByIdService>();
            services.AddScoped<IAsyncQueryHandler<UserUpdateQuery, MeUser>, UserUpdateService>();
            services.AddScoped<IAsyncQueryHandler<UsersUpdateOktaTokenQuery, MeUser>, UserUpdateOktaTokenService>();

            // Used for roles; but is being abused to pass null and get all users.
            services.AddScoped<IAsyncQueryHandler<UsersRetrievalQuery, IEnumerable<MeUser>>, UsersRetrievalService>();

            // Location Services
            services.AddScoped<IAsyncQueryHandler<LocationRetrievalByIdQuery, Location>, LocationIdService>();
            services.AddScoped<IAsyncQueryHandler<LocationsRetrievalByQuery, IEnumerable<Location>>, LocationsQueryService>();
        }

        /// <summary>
        ///     Configure basic authentication so we can use tokens.
        /// Configure basic authentication so we can use tokens.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="oktaSettings">Okta Settings.</param>
        private void ConfigureAuthentication(IServiceCollection services, OktaSettings oktaSettings)
        {
            var oktaTokenExpiry = Int32.Parse(oktaSettings.LocalTokenExpiryTimeMinutes);
            var provider = services.BuildServiceProvider();
            var tokenService = provider.GetRequiredService<ITokenService>();

            var userConnectionSetting = new UserConnectionSettings(
                new Uri(Configuration["CosmosDB:URL"]),
                Configuration["CosmosDB:PrimaryKey"],
                Configuration["CosmosDB:DatabaseId"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = oktaSettings.Authority;
                    options.Audience = oktaSettings.Audience;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(
                        new OktaJwtSecurityTokenHandler(
                            tokenService,
                            new JwtSecurityTokenHandler(),
                            new UserUpdateOktaTokenService(new DatabaseAccess(new DocumentClientFactory()), userConnectionSetting),
                            new UsersRetrievalByOktaTokenService(new DatabaseAccess(new DocumentClientFactory()), userConnectionSetting),
                            new UserRetrievalByEmailService(new DatabaseAccess(new DocumentClientFactory()), userConnectionSetting),
                            oktaTokenExpiry));
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

        /// <summary>
        /// Creates the information for API versions.
        /// </summary>
        /// <param name="description">The Description.</param>
        /// <param name="apiTitle">The API Title</param>
        /// <param name="apiDescription">The API Description</param>
        /// <returns>Info for Swagger</returns>
        private static Info CreateInfoForApiVersion(ApiVersionDescription description, string apiTitle, string apiDescription)
        {
            var info = new Info
            {
                Title = $"{apiTitle} {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = apiDescription
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }

        /// <summary>
        /// Configure Authorization.
        /// </summary>
        /// <param name="services">Services.</param>
        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddSingleton<IRolePermissions, RolePermissions>();

            services.AddAuthorization(options =>
            {
                foreach (var permission in (Common.Authorization.Permission[])Enum.GetValues(typeof(Common.Authorization.Permission)))
                {
                    options.AddPolicy($"HasPermission={permission}", policy =>
                    {
                        policy.Requirements.Add(new PermissionRequirement(permission));
                    });
                }
            });

            // Needs to be scoped since it takes scoped parameters.
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IAuthorizationHandler, DocumentPermissionHandler>();
        }
    }
}
