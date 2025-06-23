using My.Extensions.Localization.ReportMissingKeys.Implementations;
using My.Extensions.Localization.ReportMissingKeys.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace My.Extensions.Localization.ReportMissingKeys.TagHelpers
{

    public class MissingKeysTagHelper : TagHelper
    {
        private readonly ReportMissingStringLocalizerFactory _factory;

        private readonly string _reportPath;

        public MissingKeysTagHelper(IStringLocalizerFactory factory, IOptions<ReportOptions> options)
        {
            if (factory is ReportMissingStringLocalizerFactory rmslf) 
            {
                _factory = rmslf;
            }
            else
            {
                throw new InvalidOperationException($"{ nameof(MissingKeysTagHelper) } cannot be used without { nameof(ReportMissingStringLocalizerFactory) }");
            }

            _reportPath = options.Value.ReportPath;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var l = _factory.GetMissingResources().Length;
            output.TagName = l == 0 ? "span" : "a";
            output.Content.SetContent($"{ l } keys missing");
            output.TagMode = TagMode.StartTagAndEndTag;
            if (l >= 0) {
                output.Attributes.Add("href", $"/{ _reportPath }");
            }
        }
    }
}