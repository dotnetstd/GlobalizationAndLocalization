//using System;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Localization;

//namespace SimplCommerce.Module.Localization
//{
//    public class EfStringLocalizerFactory : IStringLocalizerFactory
//    {
//        private IMemoryCache _resourcesCache;
//        private readonly IServiceProvider _serviceProvider;

//        public EfStringLocalizerFactory(IServiceProvider serviceProvider, IMemoryCache resourcesCache)
//        {
//            _serviceProvider = serviceProvider;
//            _resourcesCache = resourcesCache;
//        }

//        public IStringLocalizer Create(Type resourceSource)
//        {
//            return new EfStringLocalizer(_serviceProvider, _resourcesCache);
//        }

//        public IStringLocalizer Create(string baseName, string location)
//        {
//            return new EfStringLocalizer(_serviceProvider, _resourcesCache);
//        }
//    }
//}


//namespace DigitalShop.Services
//{
//    public class EFStringLocalizerFactory : IStringLocalizerFactory
//    {
//        private readonly LocalizationDbContext _db;

//        public EFStringLocalizerFactory()
//        {
//            _db = new LocalizationDbContext();
//            // Here we define all available languages to the app
//            // available languages are those that have a json and cs file in
//            // the Languages folder
//            _db.AddRange(
//                new Culture
//                {
//                    Name = "en-US",
//                    Resources = en_US.GetList()
//                },
//                new Culture
//                {
//                    Name = "fa-IR",
//                    Resources = fa_IR.GetList()
//                }
//            );
//            _db.SaveChanges();
//        }

//        public IStringLocalizer Create(Type resourceSource)
//        {
//            return new EFStringLocalizer(_db);
//        }

//        public IStringLocalizer Create(string baseName, string location)
//        {
//            return new EFStringLocalizer(_db);
//        }
//    }

//    public class EFStringLocalizer : IStringLocalizer
//    {
//        private readonly LocalizationDbContext _db;

//        public EFStringLocalizer(LocalizationDbContext db)
//        {
//            _db = db;
//        }

//        public LocalizedString this[string name]
//        {
//            get
//            {
//                var value = GetString(name);
//                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
//            }
//        }

//        public LocalizedString this[string name, params object[] arguments]
//        {
//            get
//            {
//                var format = GetString(name);
//                var value = string.Format(format ?? name, arguments);
//                return new LocalizedString(name, value, resourceNotFound: format == null);
//            }
//        }

//        public IStringLocalizer WithCulture(CultureInfo culture)
//        {
//            CultureInfo.DefaultThreadCurrentCulture = culture;
//            return new EFStringLocalizer(_db);
//        }

//        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
//        {
//            return _db.Resources
//                .Include(r => r.Culture)
//                .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//                .Select(r => new LocalizedString(r.Key, r.Value, true));
//        }

//        private string GetString(string name)
//        {
//            return _db.Resources
//                .Include(r => r.Culture)
//                .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//                .FirstOrDefault(r => r.Key == name)?.Value;
//        }
//    }

//    public class EFStringLocalizer<T> : IStringLocalizer<T>
//    {
//        private readonly LocalizationDbContext _db;

//        public EFStringLocalizer(LocalizationDbContext db)
//        {
//            _db = db;
//        }

//        public LocalizedString this[string name]
//        {
//            get
//            {
//                var value = GetString(name);
//                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
//            }
//        }

//        public LocalizedString this[string name, params object[] arguments]
//        {
//            get
//            {
//                var format = GetString(name);
//                var value = string.Format(format ?? name, arguments);
//                return new LocalizedString(name, value, resourceNotFound: format == null);
//            }
//        }

//        public IStringLocalizer WithCulture(CultureInfo culture)
//        {
//            CultureInfo.DefaultThreadCurrentCulture = culture;
//            return new EFStringLocalizer(_db);
//        }

//        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
//        {
//            return _db.Resources
//                .Include(r => r.Culture)
//                .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//                .Select(r => new LocalizedString(r.Key, r.Value, true));
//        }

//        private string GetString(string name)
//        {
//            return _db.Resources
//                .Include(r => r.Culture)
//                .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name)
//                .FirstOrDefault(r => r.Key == name)?.Value;
//        }
//    }
//}