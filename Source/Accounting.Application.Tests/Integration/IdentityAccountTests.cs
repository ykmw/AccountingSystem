using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Accounting.Application.Tests.Helpers;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Accounting.Application.Tests.Integration
{
    public class IdentityAccountTests
        : IClassFixture<AccountingApplicationFactory<Startup>>
    {
        public const string LoginUserName = "logintest@test.com";
        public const string RegisterUserName = "registertest@test.com";
        public const string Password = "v@l1DPassword";

        private readonly AccountingApplicationFactory<Startup> _factory;

        public IdentityAccountTests(AccountingApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanRegister()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var registerPage = await client.GetAsync("/Identity/Account/Register");
            var content = await HtmlHelpers.GetDocumentAsync(registerPage);

            var formContent = new Dictionary<string, string>()
            {
                { "Input.FirstName", "xxxx" },
                { "Input.LastName", "xxxx" },
                { "Input.Email", RegisterUserName },
                { "Input.PhoneNumberPrefix", "021" },
                { "Input.PhoneNumber", "5551234" },
                { "Input.Password", Password },
                { "Input.ConfirmPassword", Password }
            };

            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form")!,
                (IHtmlButtonElement)content.QuerySelector("button")!,
                formContent);

            Assert.Equal(HttpStatusCode.OK, registerPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Identity/Account/RegisterConfirmation", response.Headers.Location?.OriginalString);
            Assert.Contains(RegisterUserName, response.Headers.Location?.OriginalString);
        }

        [Fact]
        public async Task CanLogin()
        {
            await _factory.CreateRegisteredUser(LoginUserName, Password);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var loginPage = await client.GetAsync("/Identity/Account/Login");
            var content = await HtmlHelpers.GetDocumentAsync(loginPage);

            var formContent = new Dictionary<string, string>()
            {
                { "Input.Email", LoginUserName },
                { "Input.Password", Password },
                { "Input.RememberMe", "false" }
            };

            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form")!,
                (IHtmlButtonElement)content.QuerySelector("button")!,
                formContent);

            Assert.Equal(HttpStatusCode.OK, loginPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location?.OriginalString);
        }
    }
}
