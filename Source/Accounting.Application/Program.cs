using System;
using Accounting.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Accounting.Application
{
    class Program
    {
        private Program() { }

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            ApplyMigrationsInDevelopment(host);

            // Inserts the seed data without running the app.
            if (args.Length == 1 && args[0].ToLower() == "/seed")
            {
                RunSeeding(host);
            }
            else
            {
                host.Run();
            }
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory!.CreateScope();
            var seeder = scope.ServiceProvider.GetService<AccountingSeeder>();
            seeder!.Seed();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static void ApplyMigrationsInDevelopment(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            if (!env.IsDevelopment()) return;

            var db = scope.ServiceProvider.GetRequiredService<AccountingDbContext>();
            db.Database.Migrate();
        }
    }
}
