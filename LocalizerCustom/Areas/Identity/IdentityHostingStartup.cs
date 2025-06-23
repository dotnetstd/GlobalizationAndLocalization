using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AspNetCorePagesIdentity.Areas.Identity.IdentityHostingStartup))]
namespace AspNetCorePagesIdentity.Areas.Identity
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