using Accounting.Data;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Accounting.Data.Tests
{
    [TestClass]
    public class OrganisationRepositoryTests
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions = default!;
        private readonly DbContextOptionsBuilder<AccountingDbContext> _builder;
        protected static DbContextOptionsBuilder<AccountingDbContext> OnConfiguring(DbContextOptionsBuilder<AccountingDbContext> optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                       .AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level >= LogLevel.Information);
                });

                optionsBuilder.UseInMemoryDatabase("InMemortyTests")
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();

                return optionsBuilder;
            }
            return optionsBuilder;
        }
        public OrganisationRepositoryTests()
        {
            _builder = OnConfiguring(new DbContextOptionsBuilder<AccountingDbContext>());
        }

        [TestMethod]
        public void OrganisationRepositoryLoadsRelatedData()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder.Options, _operationalStoreOptions);

            // Add a user
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 1);
            var user = context.User.First();

            // Add an Organisation
            user.RegisterOrganisation(OrganisationFactory.SimpleValid().Build());
            context.SaveChanges();

            // Act
            var sut = new OrganisationRepository(context);
            var loadedUser = sut.FindUser(user.UserName);

            // Assert
            Assert.IsTrue(loadedUser?.IsOrganisationRegistered);
        }
    }
}
