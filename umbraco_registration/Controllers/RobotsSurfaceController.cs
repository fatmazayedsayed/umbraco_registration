using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using umbraco_registration.Extensions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace umbraco_registration.Controllers
{
    public class RobotsSurfaceController : SurfaceController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly Settings _settings;

        public RobotsSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, IHttpContextAccessor httpContextAccessor, IUmbracoContextFactory umbracoContextFactory, IOptions<Settings> settings)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _umbracoContextFactory = umbracoContextFactory;
            _settings = settings.Value;
        }

        [Route("robots.txt")]
        public IActionResult Index()
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot()
                .FirstOrDefault(x => x.ContentType.Alias == "home");
            var allowItems = _settings.Robots?.Allow;
            var disallowItems = _settings.Robots?.Disallow;

            var stringBuilder = new StringBuilder().AppendLine("User-agent: *");

            if (disallowItems != null && !disallowItems.Contains("/") && allowItems != null && allowItems.Any())
            {
                foreach (var allow in allowItems)
                {
                    stringBuilder.AppendLine("Allow: " + allow);
                }
                stringBuilder.AppendLine("Sitemap: https://" + _httpContextAccessor.HttpContext?.Request.Host.Host + "/sitemap.xml");

                if (rootNode?.GetProductsPage() != null)
                {
                    stringBuilder.AppendLine("Sitemap: https://" + _httpContextAccessor.HttpContext?.Request.Host.Host + "/products.xml");
                }

                if (rootNode?.GetBlogPage() != null)
                {
                    stringBuilder.AppendLine("Sitemap: https://" + _httpContextAccessor.HttpContext?.Request.Host.Host + "/blogposts.xml");
                    stringBuilder.AppendLine("Sitemap: https://" + _httpContextAccessor.HttpContext?.Request.Host.Host + "/blog/feed.atom");
                    stringBuilder.AppendLine("Sitemap: https://" + _httpContextAccessor.HttpContext?.Request.Host.Host + "/blog/feed.rss");
                }
            }

            if (disallowItems != null && disallowItems.Any())
            {
                foreach (var disallow in disallowItems)
                {
                    stringBuilder.AppendLine("Disallow: " + disallow);
                }
            }

            return Content(stringBuilder.ToString());
        }
    }
}
