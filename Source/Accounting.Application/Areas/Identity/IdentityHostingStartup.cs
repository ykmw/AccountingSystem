using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Accounting.Application.Areas.Identity.IdentityHostingStartup))]
namespace Accounting.Application.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
