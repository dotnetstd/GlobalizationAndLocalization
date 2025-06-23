using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace LoggingCachingLocalization
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.ConfigureLogging((hostingContext, logging) =>
			{
				logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				logging.AddConsole();
				logging.AddConsole(options => options.IncludeScopes = true);
				logging.AddDebug();

				logging.AddFilter<ConsoleLoggerProvider>("DotNETCoreDay.Controllers.HomeController", LogLevel.Information);
				logging.SetMinimumLevel(LogLevel.Error);
			})
			.UseStartup<Startup>();
	}
}
