using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Accounting.Application.Tests.Helpers
{
    internal class EmailSenderSpy : IEmailSender
    {
        internal string Email { get; set; } = string.Empty;
        internal string Subject { get; set; } = string.Empty;
        internal string HtmlMessage { get; set; } = string.Empty;

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Email = email;
            Subject = subject;
            HtmlMessage = htmlMessage;

            return Task.CompletedTask;
        }
    }
}
