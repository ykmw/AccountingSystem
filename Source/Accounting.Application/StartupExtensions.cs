using System.Linq;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Accounting.Data;
using Accounting.Domain;
using Accounting.Services;


namespace Accounting.Application
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Add the per-request database context and the developer database exception filter.
        /// </summary>
        /// <remarks>
        /// Sets up Sqlite for the Development environment and Sql Server for the others.
        /// This makes it easier to get started with the project as there is no need to 
        /// setup a local sql server and override the default connection string.
        /// </remarks>
        public static void AddDatabase(this IServiceCollection services, string defaultDatabaseType, IConfigurationSection connectionString, bool isDevelopment = false)
        {
            services.AddDbContext<AccountingDbContext>(options =>
            {
                if (isDevelopment)
                {
                    services.AddDatabaseDeveloperPageExceptionFilter();

                    // NOTE: Might be better to implement this with an interface.
                    switch (defaultDatabaseType)
                    {
                        case "SQLite":
                            options.UseSqlite(connectionString.GetSection(defaultDatabaseType).Value, x => x.MigrationsAssembly("Accounting.Data.LocalMigrations"));
                            break;
                        case "SQLServer":
                            options.UseSqlServer(connectionString.GetSection(defaultDatabaseType).Value);
                            break;
                        default:
                            options.UseSqlite(connectionString.GetSection(defaultDatabaseType).Value, x => x.MigrationsAssembly("Accounting.Data.LocalMigrations"));
                            break;
                    }

                    options.EnableSensitiveDataLogging();
                }
                else
                {
                    // The Default connection for Test / Production environments
                    options.UseSqlServer(connectionString.GetSection("SQLServer").Value);
                }
            });
        }
        /// <summary>
        /// Adds the SendGrid Email Sender to the pipeline, for sending email to users.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="sendGridConfig"></param>
        public static void AddEmailSender(this IServiceCollection services, IConfigurationSection sendGridConfig)
        {
            // Add the Email Sender - SendGrid - and Options
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<SendGridOptions>(sendGridConfig);
        }
        /// <summary>
        /// Add ASP.Net Core Identity and Identity Server 4
        /// </summary>
        /// <remarks>
        /// ASP.Net Core Identity handles user authentication as well as user registration and management
        /// Scaffold Identity in ASP.NET Core projects
        /// see: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-5.0&tabs=visual-studio
        /// </remarks>
        /// <param name="services"></param>
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<AccountingDbContext>();

            services.AddRazorPages();

            // Added so we can get the User claims
            services.AddHttpContextAccessor();

            // Add IdentityServer4 which handles the Open ID Connect protocol used by the Angular client
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddApiAuthorization<User, AccountingDbContext>(options =>
                {
                    options.ApiResources
                        .First() // represents the configured default Api
                        .UserClaims
                        .Add(JwtClaimTypes.Name);
                });
        }

        /// <summary>
        /// Configure authentication and authorization using Open ID Connect.
        /// </summary>
        /// <remarks>
        /// The 4 components for authentication and authorization are:
        ///   - user
        ///   - client (the Angular app)
        ///   - identity provider (ASP.Net Core Identity with IdentityServer4), and
        ///   - resource server (the invoice and registration api)
        ///   
        /// In this project, the identity provider and resource server are hosted together.
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="authority"></param>
        /// <param name="environment"></param>
        public static void AddApi(this IServiceCollection services, string authority, IWebHostEnvironment environment)
        {
            // Add Controllers with API
            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    // Adding an empty Authorise filter means that any Authenticated user is valid.
                    options.Filters.Add(new AuthorizeFilter());
                });

            // For Azure App Service deployments on Linux
            if (!environment.IsDevelopment())
            {
                services.Configure<JwtBearerOptions>(
                    IdentityServerJwtConstants.IdentityServerJwtBearerScheme, options =>
                {
                    options.Authority = authority;
                });
            }

            // Configures the app to validate JWT tokens produced by IdentityServer
            // registers an <<ApplicationName>>API API resource with IdentityServer
            // with a default scope of <<ApplicationName>>API
            // and configures the JWT Bearer token middleware
            // to validate tokens issued by IdentityServer for the app.
            services.AddAuthentication()
                .AddIdentityServerJwt();

            // Line below solves huge Identity Server 4 bug!!! in the MS SPA template - Wasted weeks on this!!!
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Authenticate requests to the Api
            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = true;
                    options.Audience = "Accounting.ApplicationAPI";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
                });

            // https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html
            // Adds Authorization policy to make sure the token has scope 'Accounting.ApplicationAPI openid profile'
            services.AddAuthorization(options =>
            {
                // Registers the Policy with Authorization middleware
                options.AddPolicy("Accounting.ApplicationAPI", policy =>
                {
                    // Policy specifies that any Authenticated user with Accounting.ApplicationAPI scope is valid.
                    policy.RequireAuthenticatedUser();
                });
            });

            // Add OpenAPI
            if (!environment.IsProduction())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounting.Application", Version = "v1" });
                });
            }
        }

        public static void AddClient(this IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "./Accounting.Ng/dist";
            });
        }
    }
}
