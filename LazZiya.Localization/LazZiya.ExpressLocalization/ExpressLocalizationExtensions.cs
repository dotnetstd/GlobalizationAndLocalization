﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using LazZiya.TagHelpers;

namespace LazZiya.ExpressLocalization
{
    /// <summary>
    /// Add localization support for dot net core web apps with one simple step.
    /// <para>download necessary resource files from: https://github.com/LazZiya/ExpressLocalization.Resources</para>
    /// </summary>
    public static class ExpressLocalizationExtensions
    {
        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TLocalizationResource">Type of localization resource, this resource should contain all localization messages for Identity, ModelBinding, DataAnnotations localized validiation messages and view localizations as well.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            return builder
                .AddViewLocalization(ops => { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TLocalizationResource>()
                .ExAddModelBindingLocalization<TLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name)
                .ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TExpressLocalizationResource">Type of ExpressLocalizationResource, this resource should contain Identity, ModelBinding and DataAnnotations localized validiation messages.</typeparam>
        /// <typeparam name="TViewLocalizationResource">Type of ViewLocalizationResource, this resource is a shared view localization resource.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TExpressLocalizationResource, TViewLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TExpressLocalizationResource : class 
            where TViewLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            return builder
                .AddViewLocalization(ops=> { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TViewLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TExpressLocalizationResource>()
                .ExAddModelBindingLocalization<TExpressLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TExpressLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name)
                .ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add all below localization settings with one step;
        /// <para>define supported cultures adn default culture</para>
        /// <para>Add global route template for culture parameter e.g.: http://localhost:1234/{culture}/xxx </para>
        /// <para>Add route value request culture provider</para>
        /// <para>Add view localization using shared resource</para>
        /// <para>Add DataAnnotations localization</para>
        /// <para>Add ModelBinding localization</para>
        /// <para>Add IdentityError localization</para>
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TIdentityErrorsLocalizationResource">Type of localization resource for Identity error messages.</typeparam>
        /// <typeparam name="TModelBindingLocalizationResource">Type of localization resource for model binding error messages.</typeparam>
        /// <typeparam name="TDataAnnotationsLocalizationResource">Type of localization resource for data annotations.</typeparam>
        /// <typeparam name="TViewLocalizationResource">Type of localization shared resource for razor views.</typeparam>
        /// <param name="builder"></param>
        /// <param name="options">ExpressLocalizationOptions such as supported cultures and default culture</param>
        /// <returns></returns>
        public static IMvcBuilder AddExpressLocalization<TViewLocalizationResource, TDataAnnotationsLocalizationResource, TModelBindingLocalizationResource, TIdentityErrorsLocalizationResource>(this IMvcBuilder builder, Action<ExpressLocalizationOptions> options)
            where TViewLocalizationResource : class
            where TDataAnnotationsLocalizationResource : class
            where TModelBindingLocalizationResource : class
            where TIdentityErrorsLocalizationResource : class
        {
            var _options = new ExpressLocalizationOptions();
            options.Invoke(_options);

            var _ops = new RequestLocalizationOptions();
            _options.RequestLocalizationOptions.Invoke(_ops);

            builder.Services.Configure<RequestLocalizationOptions>(_options.RequestLocalizationOptions);

            return builder
                .AddViewLocalization(ops => { ops.ResourcesPath = _options.ResourcesPath; })
                .ExAddSharedCultureLocalizer<TViewLocalizationResource>()
                .ExAddDataAnnotationsLocalization<TDataAnnotationsLocalizationResource>()
                .ExAddModelBindingLocalization<TModelBindingLocalizationResource>()
                .ExAddIdentityErrorMessagesLocalization<TIdentityErrorsLocalizationResource>()
                .ExAddRouteValueRequestCultureProvider(_ops.SupportedCultures, _ops.DefaultRequestCulture.Culture.Name)
                .ExAddClientSideLocalizationValidationScripts();
        }

        /// <summary>
        /// Add DataAnnotatons localization to the project.
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TDataAnnotationsLocalizationResource">Type of DataAnnotations localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddDataAnnotationsLocalization<TDataAnnotationsLocalizationResource>(this IMvcBuilder builder) where TDataAnnotationsLocalizationResource : class
        {
            builder.AddDataAnnotationsLocalization(x =>
            {
                var type = typeof(TDataAnnotationsLocalizationResource);
                var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var localizer = factory.Create(type.Name, assemblyName.Name);

                x.DataAnnotationLocalizerProvider = (t, f) => localizer;
            });

            return builder;
        }

        /// <summary>
        /// Add ModelBinding localization to the project.
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TModelBindingLocalizationResource">Type of ModelBinding localization resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddModelBindingLocalization<TModelBindingLocalizationResource>(this IMvcBuilder builder) where TModelBindingLocalizationResource : class
        {
            builder.AddMvcOptions(ops =>
            {
                ops.ModelBindingMessageProvider.SetLocalizedModelBindingErrorMessages<TModelBindingLocalizationResource>();
            });

            return builder;
        }

        /// <summary>
        /// Add shared locaization settings for views
        /// <para>Sample resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TViewLocalizationResource">Type of shared localization resource for razor pages</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddSharedCultureLocalizer<TViewLocalizationResource>(this IMvcBuilder builder) where TViewLocalizationResource : class
        {
            builder.Services.AddSingleton<SharedCultureLocalizer>((x) => new SharedCultureLocalizer(x.GetRequiredService<IHtmlLocalizerFactory>(), typeof(TViewLocalizationResource)));

            return builder;
        }

        /// <summary>
        /// Add IdentityErrors localization to the project
        /// <para>Related resource files can be downloaded from: https://github.com/LazZiya/ExpressLocalization.Resources </para>
        /// </summary>
        /// <typeparam name="TIdentityErrorsLocalizationResource">Type of IdentityErro messages localizaton resource</typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddIdentityErrorMessagesLocalization<TIdentityErrorsLocalizationResource>(this IMvcBuilder builder) where TIdentityErrorsLocalizationResource : class
        {
            builder.Services.AddScoped<IdentityErrorDescriber, IdentityErrorsLocalizer<TIdentityErrorsLocalizationResource>>(ops =>
                new IdentityErrorsLocalizer<TIdentityErrorsLocalizationResource>());

            return builder;
        }

        /// <summary>
        /// Add client side libraries for valdiating localized inputs like decimal numbers
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder ExAddClientSideLocalizationValidationScripts(this IMvcBuilder builder)
        {
            builder.Services.AddTransient<ITagHelperComponent, LocalizationValidationScriptsTagHelperComponent>();

            return builder;
        }

        /// <summary>
        /// Add RouteValueRequestCultureProvider, so localization will be based on route value {culture}
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="cultures">List of supported cultures</param>
        /// <param name="defaultCulture">default culture name</param>
        /// <returns></returns>
        public static IMvcBuilder ExAddRouteValueRequestCultureProvider(this IMvcBuilder builder, IList<CultureInfo> cultures, string defaultCulture)
        {
            builder.Services.Configure<RequestLocalizationOptions>(ops=> {
                ops.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(cultures, defaultCulture));
            });

            builder.AddRazorPagesOptions(x =>
            {
                x.Conventions.Add(new RouteTemplateModelConvention());
            });

            return builder;
        }
    }
}
