using System;
using System.IO;
using System.Threading.Tasks;
using My.Extensions.Localization.ReportMissingKeys.Implementations;
using My.Extensions.Localization.ReportMissingKeys.Interfaces;
using My.Extensions.Localization.ReportMissingKeys.Options;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace My.Extensions.Localization.ReportMissingKeys.Middlewares
{
    public class ReportMissingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ReportMissingStringLocalizerFactory _factory;

        private readonly IOutputFormatter _formatter;

        private readonly TemplateMatcher _templateMatcher;

        private bool _isReportingEnabled => _templateMatcher != null;

        public ReportMissingMiddleware(RequestDelegate next, IOptions<ReportOptions> options, IStringLocalizerFactory factory, IOutputFormatter formatter)
        {
            _next = next;
            var reportPath = options.Value.ReportPath;
            if (string.IsNullOrEmpty(reportPath))
            {
                return;
            }
            _templateMatcher = new TemplateMatcher(TemplateParser.Parse($"{reportPath}/{{filename?}}"), new RouteValueDictionary());

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            _formatter = formatter;

            if (factory is ReportMissingStringLocalizerFactory rmslf)
            {
                _factory = rmslf;
            }
            else
            {
                throw new InvalidOperationException($"{ nameof(ReportMissingMiddleware) } cannot be used without { nameof(ReportMissingStringLocalizerFactory) }");
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var routeValues = new RouteValueDictionary();
            if (_isReportingEnabled && _templateMatcher.TryMatch(context.Request.Path, routeValues))
            {
                var keys = _factory.GetMissingResources();

                using (var ms = new MemoryStream())
                {
                    await _formatter.WriteAsync(ms, keys);
                    ms.Position = 0;

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = _formatter.ContentTypeProduced;

                    await ms.CopyToAsync(context.Response.Body);
                }
            }
            else
            {
                // Call the next delegate/middleware in the pipeline
                await this._next(context);
            }
        }
    }
}