using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounting.Application;
using Accounting.Application.Models;
using System.Diagnostics.CodeAnalysis;
using Accounting.Domain;
using Accounting.Data;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;
using Accounting.Data.Tests;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Accounting.Integration.Tests
{
    /// <summary>
    /// Architecture notes
    /// Integration tests follow a sequence of events that include the usual Arrange, Act, and Assert test steps:
    /// The SUT's web host is configured.
    /// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
    /// 
    /// A test server client is created to submit requests to the app.
    /// The Arrange test step is executed: The test app prepares a request.
    /// The Act test step is executed: The client submits the request and receives the response.
    /// The Assert test step is executed: The actual response is validated as a pass or fail based on an expected response.
    /// 
    /// The process continues until all of the tests are executed.
    /// The test results are reported.
    /// 
    /// Ref Aysnc best practice: 
    /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
    /// </summary>
    [TestClass]
    [SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "Block syntax is nicer here.")]
    public class ControllerTests
    {
        private static readonly WebApplicationFactory<Startup> _factory = new();
        private readonly HttpClient _client = _factory.CreateClient(new WebApplicationFactoryClientOptions());
        private readonly Uri _uri = new("https://localhost:5001");
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;
        private readonly DbContextOptionsBuilder<AccountingDbContext>? _builder;

        protected static DbContextOptionsBuilder<AccountingDbContext> OnConfiguring(DbContextOptionsBuilder<AccountingDbContext> optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AccountsDb_Accounts2;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                return optionsBuilder;
            }
            return optionsBuilder;
        }

        public ControllerTests()
        {
            // setup HttpClient instance
            _client.BaseAddress = _uri;
            _client.Timeout = new TimeSpan(0, 0, 30);

            // setup DB context to remove tests data dependency
            _operationalStoreOptions = Options.Create<OperationalStoreOptions>(new OperationalStoreOptions());
            _builder = OnConfiguring(new DbContextOptionsBuilder<AccountingDbContext>());
        }

        #region Controller Tests
        /// <summary>
        /// GetAllCustomers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Ignore] // test is not working yet.
        public async Task CanReadAllCustomerAsync()
        {
            // discover endpoints from metadata
            var disco = await _client.GetDiscoveryDocumentAsync(_uri.OriginalString);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // Request token
            // GrantType = "client_credentials",
            // Scope = "Accounting.ApplicationAPI openid profile"
            _client.SetBearerToken("token");
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                Scope = "Accounting.ApplicationAPI openid profile",
                ClientId = "NZBAAccounting"
            });

            if (tokenResponse.IsError)
            {
                Assert.Fail(tokenResponse.Error.ToString());
            }

            // call api
            _client.SetBearerToken(tokenResponse.AccessToken);

            var response = await _client.GetAsync("https://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Assert.Fail(response.StatusCode.ToString());
            }
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));

            // Arrange
            using var context = new AccountingDbContext(_builder!.Options, _operationalStoreOptions);

            // Create a new User
            var user = Data.UserFactory.GenerateUser();
            context.Add(user);
            context.SaveChanges();

            // Register a new Organisation
            var org = Data.OrganisationFactory.SimpleValid().Build();
            user.RegisterOrganisation(org);
            context.SaveChanges();

            // Add Invoice to User Organisation
            var invoice = new Invoice();
            invoice.InvoiceRef = "123";
            invoice.PurchaseOrder = "ABC1";
            invoice.Date = "Now";
            invoice.OrganisationId = user!.Organisation.Id;
            context.Add(invoice);
            context.SaveChanges();

            // Add Customer to Invoice
            var customer = new Customer(Data.OrganisationFactory.GenerateProperty("OrganisationName"));
            customer.ContactName = SeedDataHelper.GenerateProperty("FirstName");
            customer.ContactEmail = SeedDataHelper.GenerateProperty("Email");
            customer.Address = AddressFactory.FullAddress().Build();
            customer.Phone = Data.OrganisationFactory.GeneratePhone();

            invoice.Customer = customer;
            context.SaveChanges();

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Customer");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _cancellationTokenSource.CancelAfter(2000);
            var response2 = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            Assert.IsNotNull(response2);
            // Assert
            Assert.IsTrue(response.EnsureSuccessStatusCode().IsSuccessStatusCode); // Status Code 200-299
            Assert.AreNotEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.BadRequest, response.StatusCode);

            // NOTE: using a stream is faster, implemented here with extension method to save code
            var stream = await response.Content.ReadAsStreamAsync();
            var customers = stream.ReadAndDeserializeFromJson<List<CustomerDto>>();
            Assert.IsNotNull(customers);
        }

        /// <summary>
        /// GetCustomer
        /// </summary>
        /// <returns></returns>
        [Ignore] // test is not working yet.
        [TestMethod]
        public async Task CanReadCustomerAsync()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder!.Options, _operationalStoreOptions);

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer("HttpClient Test Customer"), 1);
            var newCustomer = await context.Customer.LastAsync();


            var customerId = newCustomer.Id;
            var serializedId = JsonConvert.SerializeObject(customerId);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Customer/{customerId}");
            request.Content = new StringContent(serializedId);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.8));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain", 0.9));
            _cancellationTokenSource.CancelAfter(2000);
            var response = await _client.SendAsync(request);

            // Assert
            Assert.IsTrue(response.EnsureSuccessStatusCode().IsSuccessStatusCode); // Status Code 200-299
            Assert.AreNotEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.BadRequest, response.StatusCode);

            // check the customer
            var customer = JsonConvert.DeserializeObject<CustomerDto>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(customer);
        }

        /// <summary>
        /// UpdateCustomer
        /// </summary>
        /// <returns></returns>
        [Ignore] // test is not working yet.
        [TestMethod]
        public async Task CanUpdateCustomerAsync()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder!.Options, _operationalStoreOptions);

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Customer("HttpClient Test Customer"), 1);
            var newCustomer = await context.Customer
                .Include(c => c.Address)
                .LastAsync();

            newCustomer.Name = "XxX";
            newCustomer.ContactName = "XxX";
            var jsonTransfer = JsonConvert.SerializeObject(newCustomer);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/Customer/{newCustomer.Id}");
            request.Content = new StringContent(jsonTransfer);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _cancellationTokenSource.CancelAfter(2000);
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, _cancellationTokenSource.Token);

            // Assert
            Assert.IsTrue(response.EnsureSuccessStatusCode().IsSuccessStatusCode); // Status Code 200-299
            Assert.AreNotEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// CreateCustomer
        /// </summary>
        /// <returns></returns>
        [Ignore] // note: test is not working yet.
        [TestMethod]
        public async Task CanCreateCustomerAsync()
        {
            // Arrange
            using var context = new AccountingDbContext(_builder!.Options, _operationalStoreOptions);

            SeedDataHelper.SeedEntity(_builder.Options, _operationalStoreOptions, new Invoice(), 1);
            var newInvoice = await context.Invoice
                .LastAsync();

            var customer = new CustomerDto("My Mate Ltd");
            customer.InvoiceId = newInvoice.Id;
            customer.ContactEmail = "sds@dsd.com";
            customer.ContactName = "sdsd";
            customer.Address = new AddressDto()
            {
                AddressLine1 = "Grove Street",
                AddressLine3 = "Pancake Park",
            };
            customer.Phone = new PhoneDto("01", "123456");
            var jsonTransfer = JsonConvert.SerializeObject(customer);

            // Act
            var serializedAccountToCreate = new StringContent(jsonTransfer);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/Customer");
            request.Content = serializedAccountToCreate;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _cancellationTokenSource.CancelAfter(2000);
            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, _cancellationTokenSource.Token);

            // Assert
            Assert.IsTrue(response.EnsureSuccessStatusCode().IsSuccessStatusCode); // Status Code 200-299
            Assert.AreNotEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreNotEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var createdCustomer = JsonConvert.DeserializeObject<CustomerDto>(await response.Content.ReadAsStringAsync());
            Assert.IsNotNull(createdCustomer);
        }
        #endregion
    }

    public static class StreamExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Can't read from this stream.");
            }

            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<T>(jsonTextReader);
        }

        public static void SerializeToJsonAndWrite<T>(this Stream stream, T objectToWrite)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new NotSupportedException("Can't write to this stream.");
            }

            using var streamWriter = new StreamWriter(stream, new UTF8Encoding(), 1024, true);
            using var jsonTextWriter = new JsonTextWriter(streamWriter);
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonTextWriter, objectToWrite);
            jsonTextWriter.Flush();
        }
    }
}
