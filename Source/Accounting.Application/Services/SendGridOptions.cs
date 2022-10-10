namespace Accounting.Services
{
    public class SendGridOptions
    {
        public string SendGridUser { get; set; } = string.Empty;
        public string SendGridKey { get; set; } = string.Empty;
        public string SendGridEmail { get; set; } = string.Empty;
    }
}
