using System.Threading.Tasks;
using Accounting.Application.Areas.Identity.Pages.Account;
using Accounting.Application.Tests.Helpers;
using Accounting.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;
using static Accounting.Application.Areas.Identity.Pages.Account.RegisterModel;

namespace Accounting.Application.Tests.Unit
{
    public class RegistrationTests
    {
        private const string ReturnUrl = "/returnUrl";

        private readonly InputModel _registration = new()
        {
            FirstName = "validFirstName",
            LastName = "validLastName",
            Email = "valid@valid.com",
            Password = "validP@ssw0rd!",
            ConfirmPassword = "validP@ssw0rd!",
            PhoneNumberPrefix = "01",
            PhoneNumber = "23456789",
        };

        private readonly string _expectedSubject
            = "NZBA Accounting Software - Verify your email address to access your account!";

        private readonly string _expectedLogo = "<img.*nzba-logo";

        private readonly RegisterModel _sut;
        private readonly EmailSenderSpy _emailSpy;

        public RegistrationTests()
        {
            _emailSpy = new EmailSenderSpy();
            var userManagerMock = Doubles.UserManagerMock<User>();

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("code");

            _sut = new RegisterModel(
                userManagerMock.Object,
                Doubles.SignInManager<User>(),
                Doubles.Logger<RegisterModel>(),
                _emailSpy);

            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);

            _sut.PageContext = new PageContext(actionContext);

            var mockUrl = new Mock<IUrlHelper>();
            mockUrl.Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>())).Returns("/page");
            mockUrl.SetupGet(x => x.ActionContext).Returns(actionContext);

            _sut.Url = mockUrl.Object;
        }

        [Fact]
        public async Task Email_Has_Recipient()
        {
            _sut.Input = _registration;

            _ = await _sut.OnPostAsync(ReturnUrl);

            Assert.Same(_registration.Email, _emailSpy.Email);
        }

        [Fact]
        public async Task Email_Has_Subject()
        {
            _sut.Input = _registration;

            _ = await _sut.OnPostAsync(ReturnUrl);

            Assert.Same(_expectedSubject, _emailSpy.Subject);
        }

        [Fact]
        public async Task Email_Contains_Name()
        {
            _sut.Input = _registration;

            _ = await _sut.OnPostAsync(ReturnUrl);

            Assert.Contains(_registration.FirstName, _emailSpy.HtmlMessage);
        }

        [Fact]
        public async Task Email_Contains_Logo()
        {
            _sut.Input = _registration;

            _ = await _sut.OnPostAsync(ReturnUrl);

            Assert.Matches(_expectedLogo, _emailSpy.HtmlMessage);
        }
    }
}
