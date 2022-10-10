using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Accounting.Data;

namespace Accounting.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds the database, uses the prefered DB type that is set in appsettings.json
            services.AddDatabase(
                Configuration.GetValue("DefaultDatabaseType", "SQLLite"),
                Configuration.GetSection("ConnectionStrings"),
                Environment.IsDevelopment());

            // Allows the /seed arg to pre populate the DB.
            services.AddTransient<AccountingSeeder>();

            // Add Interface Context
            services.AddScoped<IOrganisationRepository, OrganisationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            // Setup for Aspnet Core Identity and Identity Server
            services.AddIdentity();

            // Configuration for APIs Authentication Authorisation IdentityServer Claim and Swagger
            services.AddApi(Configuration.GetValue<string>("Authority"), Environment);

            // Adds the SendGrid service so that we can send emails.
            services.AddEmailSender(Configuration.GetSection("MessageSender:SendGrid"));

            // Sets the location to serve the Angular files from
            services.AddClient();

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            // Use OpenAPI
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("Accounting.ApplicationAPI");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "../Accounting.Ng";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer(Configuration["AngularServer"]);
                }
            });
        }
    }
}
