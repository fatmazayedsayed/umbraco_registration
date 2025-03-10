using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using umbraco_registration.Extensions;
using umbraco_registration.Interfaces;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace umbraco_registration.Controllers
{
    public class SyndicationSurfaceController : SurfaceController
    {
        private readonly ISyndicationXmlService _syndicationXmlService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly XmlWriterSettings _xmlWriterSettings = new()
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };
        public SyndicationSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ISyndicationXmlService syndicationXmlService, IUmbracoContextFactory umbracoContextFactory)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _syndicationXmlService = syndicationXmlService;
            _umbracoContextFactory = umbracoContextFactory;
        }


        [Route("blog/feed.rss")]
        [ResponseCache(Duration = 900, VaryByHeader = "Host")]
        public IActionResult Rss()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home");
            if (rootNode?.GetBlogPage() != null)
            {
                using var stream = new MemoryStream();
                using (var xmlWriter = XmlWriter.Create(stream, _xmlWriterSettings))
                {
                    var siteSettings = rootNode.GetSiteSettings();
                    var feed = _syndicationXmlService.GenerateRssXml(siteSettings?.Value<string>("feedTitle") ?? string.Empty, siteSettings?.Value<string>("feedDescription") ?? string.Empty);
                    feed.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }

            return NotFound();
        }

        [Route("blog/feed.atom")]
        [ResponseCache(Duration = 900)]
        public IActionResult Atom()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home");
            if (rootNode?.GetBlogPage() != null)
            {
                using var stream = new MemoryStream();
                using (var xmlWriter = XmlWriter.Create(stream, _xmlWriterSettings))
                {
                    var siteSettings = rootNode.GetSiteSettings();
                    var feed = _syndicationXmlService.GenerateAtomXml(siteSettings?.Value<string>("feedTitle") ?? string.Empty, siteSettings?.Value<string>("feedDescription") ?? string.Empty);
                    feed.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/atom+xml; charset=utf-8");
            }

            return NotFound();
        }
    }
}