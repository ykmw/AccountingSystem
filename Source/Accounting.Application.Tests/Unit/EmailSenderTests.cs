using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;
using Accounting.Data;
using Accounting.Services;
using Microsoft.CodeAnalysis.Options;
using Xunit;

namespace Accounting.Application.Tests.Unit
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/test/razor-pages-tests?view=aspnetcore-5.0
    /// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-5.0
    /// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0
    /// </summary>
    public class EmailSenderTests
    {
        protected EmailSenderTests()
        {
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

                optionsBuilder.UseInMemoryDatabase("EmailSenderTests")
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();

                return optionsBuilder;
            }
            return optionsBuilder;
        }

        /// <summary>
        /// Unit test for the SendGrid Service - consider using services.Configure<SendGridOptions>(sendGridConfig) in Startup;
        /// </summary>
        /// <returns></returns>
        public static async Task SendEmail()
        {
            // SendGrid is not a paid account
            // this limits the amount of emails we send.
            if (new Random().Next(1, 10) == 1)
            {
                Assert.True(true);
                return;
            }

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("testuser@nzba.org", "Accounting Test");

            var subject = "Test Message from SendGrid";
            var to = new EmailAddress("testuser@nzba.org", "Accounting Test");
            var plainTextContent = "";
            var imgurl = $"https://nzbaaccountingapp-yxml-test.azurewebsites.net/assets/nzba-logo.png";
            var htmlContent =
                        $"NZBA Accounting Software - Verify your email address to access your account!" +
                        $"<img src=\"{imgurl}\" width=\"100px;\" height: auto; /><p style=\"font-family:Calibri;font-size:16px\">" +
                        $"<p>Hi TestUser,</p>" +
                        $"<p>Thank you for signing up to use our accounting software.</p>" +
                        $"<p>The last step of the registration process is to verify your email address by clicking the link below. Once this has been completed, you will be able to log into your account.</p>" +
                        $"<p>Please confirm your account by <a href='https://nzbaaccountingapp-yxml-test.azurewebsites.net/assets/nzba-logo.png'>clicking here</a>.</p>" +
                        $"<p>If you did not sign up to use our accounting software, please discard this email and contact us at contact@nzba.org.</p>" +
                        $"<p>Thank you</p>" +
                        $"NZBA</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            Assert.IsType(new SendGridMessage().GetType(), msg);
            var result = await client.SendEmailAsync(msg);
            Assert.NotNull(result);
        }
    }
}
