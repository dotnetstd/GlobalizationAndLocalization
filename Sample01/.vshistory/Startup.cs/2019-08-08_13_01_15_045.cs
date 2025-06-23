using Localization;

using MainApp.Models.Localization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using Sample01.Data;

using System.Collections.Generic;
using System.Globalization;

namespace Sample01
{
	public class Startup
	{
		private const string faCulture = "fa";
		private const string faUICulture = "fa-IR";
		private const string enUSCulture = "en";
		private const string enUSUICulture = "en-US";

		public static string UiCulture;
		public static string CultureDirection;
		public static IStringLocalizer _e; // This is how we access language strings

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			// Returns "Welcome" for en-US and "خوش آمدید" for fa-IR
			var welcome = Startup._e["Welcome"];
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				.AddDefaultUI(UIFramework.Bootstrap4)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);





			#region ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘ Localization ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
			//services.AddScoped<ITagHelperComponent, LanguageDirectionTagHelperComponent>();
			//services.AddSingleton<IPageFactoryProvider, LocalizationFixPageFactoryProvider>();
			//services.AddMvc().AddViewLocalization();
			services.AddSession();
			services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.AddMvc(options =>
			{
				//options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
			})
			.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
			.AddDataAnnotationsLocalization();


			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
				{
					new CultureInfo(enUSUICulture),
					new CultureInfo(faUICulture),
				};

				options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSUICulture);

				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;

				//var provider = new RouteDataRequestCultureProvider();
				//provider.RouteDataStringKey = "lang";
				//provider.UIRouteDataStringKey = "lang";
				//provider.Options = options;
				//options.RequestCultureProviders = new[] { provider };

				//options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
				//{
				//    return new ProviderCultureResult("en");
				//}));
			});

			services.Configure<RouteOptions>(options =>
			{
				options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
			});

			services.AddLocalization(options => options.ResourcesPath = "Resources");

			services.AddMvc()
			.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
			.AddDataAnnotationsLocalization(options =>
			{
				options.DataAnnotationLocalizerProvider = (type, factory) =>
					factory.Create(typeof(SharedResource));
			});


			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new[]
			 {
							  new CultureInfo(faCulture),
							  new CultureInfo(faUICulture),
							  new CultureInfo(enUSCulture),
							  new CultureInfo(enUSUICulture),
					  };
				options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSUICulture);
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});


			//var cookieProvider = localizationOptions.RequestCultureProviders
			//                          .OfType<CookieRequestCultureProvider>()
			//                                 .First();

			//cookieProvider.CookieName = "UserCulture";

			//routes www.mysite.com/en-us/Home/Index or www.mysite.com/fi-fi/Home/Index?

			//var requestProvider = new RouteDataRequestCultureProvider();
			//localizationOptions.RequestCultureProviders.Insert ( 0 , requestProvider );

			//app.UseRouter ( routes =>
			//{
			//      routes.MapMiddlewareRoute ( "{culture=en-US}/{*mvcRoute}" , subApp =>
			//      {
			//            subApp.UseRequestLocalization ( localizationOptions );

			//            subApp.UseMvc ( mvcRoutes =>
			//            {
			//                  mvcRoutes.MapRoute (
			//                      name: "default" ,
			//                      template: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}" );
			//            } );
			//      } );
			//} );


			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

			services.AddPortableObjectLocalization();

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new List<CultureInfo>
				{
					new CultureInfo("en-US"),
					new CultureInfo("en"),
					new CultureInfo("fr-FR"),
					new CultureInfo("fr")
				};

				options.DefaultRequestCulture = new RequestCulture("en-US");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});


			#region snippet1
			services.AddLocalization(options => options.ResourcesPath = "Resources");

			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization();
			#endregion

			services.Configure<RequestLocalizationOptions>(options =>
			{
				var supportedCultures = new List<CultureInfo>
					{
						new CultureInfo("en-US"),
						new CultureInfo("fr")
					};

				options.DefaultRequestCulture = new RequestCulture("en-US");
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});

			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			#region ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘  Localization  ◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘
			//var supportedCultures = new[]
			//{
			//    new CultureInfo("en-US"),
			//    new CultureInfo("ar-YE")
			//};

			//var options = new RequestLocalizationOptions {
			//    DefaultRequestCulture = new RequestCulture("en-US"),
			//    SupportedCultures = supportedCultures,
			//    SupportedUICultures = supportedCultures
			//};

			//options.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider());
			//app.UseRequestLocalization(options);
			var supportedCultures = new List<CultureInfo>
			{
				new CultureInfo("en-US"),
				new CultureInfo("fa-IR"),
				new CultureInfo("fr-FR")
			};
			var options = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en-US", "en-US"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			};
			options.RequestCultureProviders = new[]
			{
				new SessionStateRequestCultureProvider() { Options = options }
			};
			app.UseRequestLocalization(options);
			app.UseSession();
			app.Use(async (context, next) =>
			{
				context.Session.SetString("culture", "ar-YE");
				context.Session.SetString("ui-culture", "ar-YE");
				await next();
			});

			var localizationOption = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(localizationOption.Value);


			// a list of all available languages
			//var supportedCultures = new List<CultureInfo>
			//{
			//    new CultureInfo("en-US"),
			//    new CultureInfo("fa-IR")
			//};

			var requestLocalizationOptions = new RequestLocalizationOptions
			{
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures,
			};
			requestLocalizationOptions.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider());
			app.UseRequestLocalization(requestLocalizationOptions);






			var localizationOptions = new RequestLocalizationOptions
			{
				SupportedCultures = new List<CultureInfo>
				{
					new CultureInfo("de-DE"),
					new CultureInfo("en-US"),
					new CultureInfo("en-GB")
				},
				SupportedUICultures = new List<CultureInfo>
				{
					new CultureInfo("de-DE"),
					new CultureInfo("en-US"),
					new CultureInfo("en-GB")
				},
				DefaultRequestCulture = new RequestCulture("en-US")
			};

			// Adding our UrlRequestCultureProvider as first object in the list
			localizationOptions.RequestCultureProviders.Insert(0, new UrlRequestCultureProvider
			{
				Options = localizationOptions
			});

			app.UseRequestLocalization(localizationOptions);

			app.UseRequestLocalization();



			var supportedCultures3 = new[]
{
				new CultureInfo("en-US"),
				new CultureInfo("fr"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en-US"),
				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = supportedCultures
			});


			#endregion
		}
	}
}
