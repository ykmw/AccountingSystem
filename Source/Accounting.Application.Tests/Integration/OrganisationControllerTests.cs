using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Accounting.Application.Models;
using Xunit;

namespace Accounting.Application.Tests.Integration
{
    public class OrganisationControllerTests
        : IAsyncLifetime, IClassFixture<AccountingApplicationFactory<Startup>>
    {
        public const string UserName = "test@test.com";

        private readonly AccountingApplicationFactory<Startup> _factory;

        public OrganisationControllerTests(AccountingApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _factory.ResetDatabase();
        }

        public async Task InitializeAsync() => await _factory.CreateRegisteredUser(UserName);
        public Task DisposeAsync() => Task.CompletedTask;

        private static OrganisationForRegistrationDto GetRegistrationDto(string shortCode = "four") =>
            new()
            {
                Name = "Name",
                AddressStreet1 = "Street1",
                AddressStreet2 = "Street2",
                AddressCityTown = "City",
                PhonePrefix = "123",
                Phone = "123456",
                ShortCode = shortCode
            };

        [Theory]
        [InlineData("api/organisation")]
        public async Task InvalidShortCodeGives400(string url)
        {
            var client = _factory.CreateClient();
            var dto = GetRegistrationDto(shortCode: "Invalid");

            var response = await client.PutAsJsonAsync(url, dto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("api/organisation")]
        public async Task UnauthenticatedUserGives404(string url)
        {
            var client = _factory.CreateClient();
            var dto = GetRegistrationDto();

            var response = await client.PutAsJsonAsync(url, dto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
