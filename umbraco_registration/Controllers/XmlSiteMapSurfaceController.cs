using System.Text;
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
    public class XmlSiteMapSurfaceController : SurfaceController
    {
        private readonly ISiteMapXmlService _xmlSiteMapXmlService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public XmlSiteMapSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ISiteMapXmlService xmlSiteMapXmlService, IUmbracoContextFactory umbracoContextFactory)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _xmlSiteMapXmlService = xmlSiteMapXmlService;
            _umbracoContextFactory = umbracoContextFactory;
        }

        [Route("sitemap.xml")]
        [ResponseCache(Duration = 900, VaryByHeader = "Host")]
        public IActionResult SiteMap()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home");

            if (rootNode != null)
            {
                return Content(_xmlSiteMapXmlService.GenerateXml(rootNode.Key), "text/xml", Encoding.UTF8);
            }

            return NotFound();
        }

        [Route("products.xml")]
        [ResponseCache(Duration = 900, VaryByHeader = "Host")]
        public IActionResult HomesSiteMap()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home")?.GetProductsPage();
            if (rootNode != null)
            {
                return Content(_xmlSiteMapXmlService.GenerateXml(rootNode.Key), "text/xml", Encoding.UTF8);
            }

            return NotFound();
        }

        [Route("blogposts.xml")]
        [ResponseCache(Duration = 900, VaryByHeader = "Host")]
        public IActionResult BlogSiteMap()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home")?.GetBlogPage();

            if (rootNode != null)
            {
                return Content(_xmlSiteMapXmlService.GenerateXml(rootNode.Key), "text/xml", Encoding.UTF8);
            }

            return NotFound();
        }
    }
}