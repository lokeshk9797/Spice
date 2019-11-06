using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Spice.Areas.Identity.IdentityHostingStartup))]
namespace Spice.Areas.Identity
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