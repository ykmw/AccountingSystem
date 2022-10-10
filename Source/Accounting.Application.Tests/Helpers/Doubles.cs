using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Accounting.Application.Tests.Helpers
{
    public static class Doubles
    {
        public static Mock<UserManager<TUser>> UserManagerMock<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var userManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<TUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return userManager;
        }

        public static SignInManager<TUser> SignInManager<TUser>() where TUser : class
        {
            var signInManager = new Mock<SignInManager<TUser>>(UserManagerMock<TUser>().Object,
                 new Mock<IHttpContextAccessor>().Object,
                 new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                 new Mock<IOptions<IdentityOptions>>().Object,
                 Logger<SignInManager<TUser>>(),
                 new Mock<IAuthenticationSchemeProvider>().Object,
                 new Mock<IUserConfirmation<TUser>>().Object);

            signInManager
                .Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            return signInManager.Object;
        }

        public static ILogger<T> Logger<T>() =>
            new Mock<ILogger<T>>().Object;
    }
}
