using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Accounting.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

#nullable disable
namespace Accounting.Application.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Prefix")]
            [StringLength(4, ErrorMessage = "Ensure Phone Prefix length is between 1-4 digits.", MinimumLength = 1)]
            [RegularExpression("([0-9]+)", ErrorMessage = "Please insert only numbers")]
            public string PhoneNumberPrefix { get; set; }

            [Required]
            [Display(Name = "Phone")]
            [StringLength(9, ErrorMessage = "Ensure Phone Prefix length is between 4-9 digits.", MinimumLength = 4)]
            [RegularExpression("([0-9]+)", ErrorMessage = "Please insert only numbers")]
            public string PhoneNumber { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User(
                    Input.Email,
                    Input.FirstName,
                    Input.LastName,
                    new Phone(Input.PhoneNumberPrefix, Input.PhoneNumber))
                {
                    // This PhoneNumber might be used for 2fa
                    PhoneNumber = ($"{Input.PhoneNumberPrefix}{Input.PhoneNumber}")
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user,
                        new System.Security.Claims.Claim("FullName", $"{Input.FirstName} {Input.LastName}"));

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);
                    var imgurl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/assets/nzba-logo.png";
                    await _emailSender.SendEmailAsync(Input.Email, "NZBA Accounting Software - Verify your email address to access your account!",
                        $"<img src=\"{imgurl}\" width=\"100px;\" height: auto; />" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">Hi {Input.FirstName},</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">Thank you for signing up to use our accounting software.</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">The last step of the registration process is to verify your email address by clicking the link below. Once this has been completed, you will be able to log into your account.</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">If you did not sign up to use our accounting software, please discard this email and contact us at contact@nzba.org.</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">Thank you</p>" +
                        $"<p style=\"font-family:Calibri;font-size:16px\">NZBA</p>");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
#nullable restore
