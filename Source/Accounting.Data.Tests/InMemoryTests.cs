using System;
using System.Linq;
using Accounting.Domain;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Accounting.Data.Tests
{
    [TestClass]
    public class InMemoryTests
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private readonly DbContextOptionsBuilder<AccountingDbContext> _builder;
        public InMemoryTests()
        {
            _operationalStoreOptions = Options.Create<OperationalStoreOptions>(new OperationalStoreOptions());
            _builder = OnConfiguring(new DbContextOptionsBuilder<AccountingDbContext>());
        }
        protected static DbContextOptionsBuilder<AccountingDbContext> OnConfiguring(DbContextOptionsBuilder<AccountingDbContext> optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                       .AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level >= LogLevel.Information)
                       .AddConsole();
                });

                optionsBuilder.UseInMemoryDatabase("InMemortyTests")
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();

                return optionsBuilder;
            }
            return optionsBuilder;
        }

        #region Organisation CRUD Tests
        [TestMethod]
        public void CanCreateOrganisation()
        {
            // Arange
            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextAct.Organisation
                .TagWith("Test: Context.Organisation.Read")
                .AsEnumerable()
                .Count();

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, Organisation.NotRegistered, 1);

            var after = contextAct.Organisation
                .TagWith("Test: Context.Organisation.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(before, after - 1);
        }
        [TestMethod]
        public void CanReadOrganisation()
        {
            using var contextArr = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextArr.Organisation
                .ToList()
                .Count;

            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, Organisation.NotRegistered, 4);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var after = contextAct.Organisation
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 4);
        }
        [TestMethod]
        public void CanUpdateOrganisation()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, OrganisationFactory.SimpleValid().Build(), 1);

            // Act
            using var contextAssert = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var organisation = contextAssert.Organisation
                .TagWith("Test: Context.Organisation.Update")
                .First();

            var originalName = organisation.Name;
            organisation.Name = SeedDataHelper.GenerateProperty("OrganisationName");
            contextAssert.SaveChanges();

            // Assert
            Assert.AreNotEqual(originalName, organisation.Name);
        }
        [TestMethod]
        public void CanDeleteOrganisation()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, Organisation.NotRegistered, 2);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var orgsBefore = contextAct.Organisation
                .TagWith("Test: Context.Organisation.Read")
                .AsEnumerable()
                .Count();


            var org = contextAct.Organisation.OrderBy(o => o.Id).Last();
            contextAct.Organisation
                .TagWith("Test: Context.Organisation.Delete");

            contextAct.Organisation.Remove(org);
            contextAct.SaveChanges();

            var orgsAfter = contextAct.Organisation
                .TagWith("Test: Context.Organisation.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(orgsBefore - 1, orgsAfter);
        }
        #endregion

        #region Customer CRUD Tests
        [TestMethod]
        public void CanCreateCustomer()
        {
            // Arange
            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextAct.Customer
                .TagWith("Test: Context.Customer.Read")
                .AsEnumerable()
                .Count();

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer(SeedDataHelper.GenerateProperty("CustomerName")), 1);

            var after = contextAct.Customer
                .TagWith("Test: Context.Customer.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(before, after - 1);
        }
        [TestMethod]
        public void CanReadCustomer()
        {
            // Arrange
            using var contextArr = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextArr.Customer
                .ToList()
                .Count;

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer(SeedDataHelper.GenerateProperty("CustomerName")), 4);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var after = contextAct.Customer
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 4);
        }
        [TestMethod]
        public void CanUpdateCustomer()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer(SeedDataHelper.GenerateProperty("CustomerName")), 1);

            // Act
            using var contextAssert = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var organisation = contextAssert.Customer
                .TagWith("Test: Context.Customer.Update")
                .First();

            var originalName = organisation.Name;
            organisation.Name = SeedDataHelper.GenerateProperty("OrganisationName");
            contextAssert.SaveChanges();

            // Assert
            Assert.AreNotEqual(originalName, organisation.Name);
        }
        [TestMethod]
        public void CanDeleteCustomer()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer(SeedDataHelper.GenerateProperty("CustomerName")), 2);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var orgsBefore = contextAct.Customer
                .TagWith("Test: Context.Customer.Read")
                .AsEnumerable()
                .Count();


            var org = contextAct.Customer.OrderBy(o => o.Id).Last();
            contextAct.Customer
                .TagWith("Test: Context.Customer.Delete");

            contextAct.Customer.Remove(org);
            contextAct.SaveChanges();

            var orgsAfter = contextAct.Customer
                .TagWith("Test: Context.Customer.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(orgsBefore - 1, orgsAfter);
        }
        #endregion

        #region Invoice CRUD Tests
        [TestMethod]
        public void CanCreateInvoice()
        {
            // Arange
            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextAct.Invoice
                .TagWith("Test: Context.Invoice.Read")
                .AsEnumerable()
                .Count();

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Invoice(), 1);

            var after = contextAct.Invoice
                .TagWith("Test: Context.Invoice.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(before, after - 1);
        }
        [TestMethod]
        public void CanReadInvoice()
        {
            // Arrange
            using var contextArr = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextArr.Invoice
                .ToList()
                .Count;

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Invoice(), 4);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var after = contextAct.Invoice
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 4);
        }
        [TestMethod]
        public void CanUpdateInvoice()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Invoice(), 1);

            // Act
            using var contextAssert = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var organisation = contextAssert.Invoice
                .TagWith("Test: Context.Invoice.Update")
                .First();

            var originalInvoiceRef = organisation.InvoiceRef;
            organisation.InvoiceRef = SeedDataHelper.GenerateProperty("OrganisationName");
            contextAssert.SaveChanges();

            // Assert
            Assert.AreNotEqual(originalInvoiceRef, organisation.InvoiceRef);
        }
        [TestMethod]
        public void CanDeleteInvoice()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Invoice(), 2);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var orgsBefore = contextAct.Invoice
                .TagWith("Test: Context.Invoice.Read")
                .AsEnumerable()
                .Count();


            var org = contextAct.Invoice.OrderBy(o => o.Id).Last();
            contextAct.Invoice
                .TagWith("Test: Context.Invoice.Delete");

            contextAct.Invoice.Remove(org);
            contextAct.SaveChanges();

            var orgsAfter = contextAct.Invoice
                .TagWith("Test: Context.Invoice.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(orgsBefore - 1, orgsAfter);
        }
        #endregion

        #region LineItem CRUD Tests
        [TestMethod]
        public void CanCreateLineItem()
        {
            // Arange
            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextAct.LineItem
                .TagWith("Test: Context.LineItem.Read")
                .AsEnumerable()
                .Count();

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new LineItem(), 1);

            var after = contextAct.LineItem
                .TagWith("Test: Context.LineItem.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(before, after - 1);
        }
        [TestMethod]
        public void CanReadLineItem()
        {
            // Arrange
            using var contextArr = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextArr.LineItem
                .ToList()
                .Count;

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new LineItem(), 4);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var after = contextAct.LineItem
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 4);
        }
        [TestMethod]
        public void CanUpdateLineItem()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new LineItem(), 1);

            // Act
            using var contextAssert = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var lineItem = contextAssert.LineItem
                .TagWith("Test: Context.LineItem.Update")
                .First();

            var originalName = lineItem.Name;
            lineItem.Name = SeedDataHelper.GenerateProperty("OrganisationName");
            contextAssert.SaveChanges();

            // Assert
            Assert.AreNotEqual(originalName, lineItem.Name);
        }
        [TestMethod]
        public void CanDeleteLineItem()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new LineItem(), 2);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var orgsBefore = contextAct.LineItem
                .TagWith("Test: Context.LineItem.Read")
                .AsEnumerable()
                .Count();


            var org = contextAct.LineItem.OrderBy(o => o.Id).Last();
            contextAct.LineItem
                .TagWith("Test: Context.LineItem.Delete");

            contextAct.LineItem.Remove(org);
            contextAct.SaveChanges();

            var orgsAfter = contextAct.LineItem
                .TagWith("Test: Context.LineItem.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(orgsBefore - 1, orgsAfter);
        }
        #endregion

        #region User CRUD Tests
        [TestMethod]
        public void CanCreateUser()
        {
            // Arange
            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextAct.User
                .TagWith("Test: Context.User.Read")
                .ToList()
                .Count;

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 1);

            var after = contextAct.User
                .TagWith("Test: Context.User.Read")
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 1);
        }
        [TestMethod]
        public void CanReadUser()
        {
            // Arrange
            using var contextArr = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var before = contextArr.User
                .ToList()
                .Count;

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 4);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var after = contextAct.User
                .ToList()
                .Count;

            // Assert
            Assert.AreEqual(before, after - 4);
        }
        [TestMethod]
        public void CanDeleteUser()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 2);

            // Act
            using var contextAct = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var orgsBefore = contextAct.User
                .TagWith("Test: Context.User.Read")
                .AsEnumerable()
                .Count();


            var org = contextAct.User.OrderBy(o => o.Id).Last();
            contextAct.User
                .TagWith("Test: Context.User.Delete");

            contextAct.User.Remove(org);
            contextAct.SaveChanges();

            var orgsAfter = contextAct.User
                .TagWith("Test: Context.User.Read")
                .AsEnumerable()
                .Count();

            // Assert
            Assert.AreEqual(orgsBefore - 1, orgsAfter);
        }
        #endregion

        #region Address CRUD Tests
        [TestMethod]
        public void Address_NoLine2()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder.Options, _operationalStoreOptions);

            // Add a user
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 1);
            var user = context.User.First();
            context.SaveChanges();

            // Add an Organisation - with no Line2 Address
            var orgFactory = new OrganisationFactory().WithThisName(OrganisationFactory.GenerateProperty("OrganisationName"));
            orgFactory.Address = AddressFactory.No_Line2().Build();

            user.RegisterOrganisation(orgFactory.Build());
            context.SaveChanges();

            // Act
            var sut = new OrganisationRepository(context);
            var loadedUser = sut.FindUser(user.UserName);

            // Assert
            Assert.AreEqual(string.Empty, loadedUser?.Organisation.Address.AddressLine2);
        }

        [TestMethod]
        public void Address_NoPostCode()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder.Options, _operationalStoreOptions);

            // Add a user
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, UserFactory.GenerateUser(), 1);
            var user = context.User.First();
            context.SaveChanges();

            // Add an Organisation - with no Line2 Address
            var orgFactory = new OrganisationFactory().WithThisName(OrganisationFactory.GenerateProperty("OrganisationName"));
            orgFactory.Address = AddressFactory.No_PostCode().Build();

            user.RegisterOrganisation(orgFactory.Build());
            context.SaveChanges();

            // Act
            var sut = new OrganisationRepository(context);
            var loadedUser = sut.FindUser(user.UserName);

            // Assert
            Assert.AreEqual(string.Empty, loadedUser?.Organisation.Address.PostCode);
        }
        #endregion

        #region Data Annotation Validation
        [TestMethod]
        public void VerifyOrganisationDataAnnotation()
        {
            // Arrange
            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, OrganisationFactory.SimpleValid().Build(), 1);

            // Act
            using var contextAssert = new AccountingDbContext(_builder.Options, _operationalStoreOptions);
            var organisation = contextAssert.Organisation.Include(o => o.Phone).First();

            // Bellow Minimum Valid GST Number
            Assert.ThrowsException<CustomApplicationException>(() => organisation.GSTNumber = "1234567");

            // Exceeds Maximum Valid GST Number
            Assert.ThrowsException<CustomApplicationException>(() => organisation.GSTNumber = "12345678910");

            // GST is set and false - for this Org
            Assert.IsFalse(organisation.IsGSTRegistered());

            organisation.GSTNumber = "123456789";
            contextAssert.SaveChanges();

            // GST is set and now true - for this Org
            Assert.IsTrue(organisation.IsGSTRegistered());

            // Bellow Minimum Valid ShortCode
            Assert.ThrowsException<CustomApplicationException>(() => organisation.ShortCode = "123");

            // Exceeds Maximum Valid ShortCode
            Assert.ThrowsException<CustomApplicationException>(() => organisation.ShortCode = "12345");

            // Bellow Minimum Valid Phone Number Prefix
            Assert.ThrowsException<CustomApplicationException>(() => organisation.Phone.PhoneNumberPrefix = "");

            // Exceeds Maximum Valid Phone Number Prefix
            Assert.ThrowsException<CustomApplicationException>(() => organisation.Phone.PhoneNumberPrefix = "12345");

            // Bellow Minimum Valid Phone Number
            Assert.ThrowsException<CustomApplicationException>(() => organisation.Phone.PhoneNumber = "411");

            // Exceeds Maximum Valid Phone Number
            Assert.ThrowsException<CustomApplicationException>(() => organisation.Phone.PhoneNumber = "1234567890");
        }

        #endregion
    }
}

