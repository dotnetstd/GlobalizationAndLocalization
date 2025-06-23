using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System;

namespace MainApp.Models.TagHelperComponents
{
    public class LanguageDirectionTagHelperComponent : TagHelperComponent
    {
        private const string LanguageDirectionAttribute = "dir";
        private const string BodyTagName = "body";
        private readonly HttpContext _httpContext;

        public LanguageDirectionTagHelperComponent(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;

        public override int Order => 1;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, BodyTagName, StringComparison.Ordinal))
            {
                // اینجا هست علت حذف
                //https://github.com/aspnet/Mvc/issues/8463
                var languageDirection = _httpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture/*GetLanguageDirection()*/.ToString().ToLower();
                if (!output.Attributes.ContainsName(LanguageDirectionAttribute))
                {
                    output.Attributes.Add(LanguageDirectionAttribute, languageDirection);
                }
                else
                {
                    output.Attributes.SetAttribute(LanguageDirectionAttribute, languageDirection);
                }
            }
        }
    }
}
