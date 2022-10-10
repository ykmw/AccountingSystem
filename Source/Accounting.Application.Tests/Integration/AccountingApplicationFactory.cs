using System.Data.Common;
using System.Threading.Tasks;
using Accounting.Data;
using Accounting.Domain;
using Accounting.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Accounting.Application.Tests.Integration
{
    /// <summary>
    /// Create a version of the application with services setup for testing.
    /// </summary>
    /// <remarks>
    /// One instance of the fixture is shared among all tests in a test class.
    /// </remarks>
    public class AccountingApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private const string DefaultUserName = "testuser@test.com";
        private const string DefaultValidPassword = "A12341Bb$";
        private DbConnection _connection;

        public AccountingApplicationFactory()
        {
            _connection = CreateDbConnection();
            CreateDatabase();
        }

        private void CreateDatabase()
        {
            using var scope = Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AccountingDbContext>();
            context.Database.EnsureCreated();
        }

        /// <summary>
        /// Create a new database.
        /// </summary>
        /// <remarks>
        /// Use to allow a test class to have a new database per test 
        /// rather than per test collection.
        /// </remarks>
        internal void ResetDatabase()
        {
            _connection.Dispose();
            _connection = CreateDbConnection();
            CreateDatabase();
        }

        internal async Task CreateRegisteredUser(string userName = DefaultUserName, string password = DefaultValidPassword)
        {
            using var scope = Services.CreateScope();
            var userManager = GetUserManager(scope);
            var user = Data.UserFactory.Create(userName);
            user.EmailConfirmed = true;

            await userManager.CreateAsync(user, password);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveService<DbContextOptions<AccountingDbContext>>();
                services.RemoveService<IEmailSender, EmailSender>();
                RemoveAuthorizeFilter();
                SetupDatabase(services);

                void RemoveAuthorizeFilter()
                    => services.RemoveInstances<IConfigureOptions<MvcOptions>>();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && _connection != null)
            {
                _connection.Dispose();
            }
        }

        private static DbConnection CreateDbConnection()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            return connection;
        }

        private void SetupDatabase(IServiceCollection services)
        {
            services.AddDbContext<AccountingDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });
        }

        private static UserManager<User> GetUserManager(IServiceScope scope)
            => scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    }
}
