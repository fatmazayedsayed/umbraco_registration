using System.Xml.Linq;
using UmbCheckout.Shared.Extensions;
using umbraco_registration.Interfaces;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace umbraco_registration.Services
{
    /// <summary>
    /// Generates an XML sitemap
    /// </summary>
    internal class SiteMapXmlService : ISiteMapXmlService
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private static readonly XNamespace Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        public SiteMapXmlService(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }

        public string GenerateXml(Guid startNode, bool includeSelf = true)
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext();
            var rootNode = context.UmbracoContext.Content?.GetById(startNode);

            if (rootNode == null)
            {
                return string.Empty;
            }

            IEnumerable<IPublishedContent> nodes;
            if (includeSelf)
            {
                nodes = rootNode.DescendantsOrSelf()
                    .Where(x => x.HasTemplate())
                    .Where(x => x.IsVisible())
                    .Where(x => x.Value<bool>("hideFromXmlSitemap") == false); 
            }
            else
            {
                nodes = rootNode.Descendants()
                    .Where(x => x.HasTemplate())
                    .Where(x => x.IsVisible())
                    .Where(x => x.Value<bool>("hideFromXmlSitemap") == false); 
            }

            XElement root = new XElement(Xmlns + "urlset");

            foreach (var node in nodes)
            {
                var urlElement = new XElement(Xmlns + "url",
                    new XElement(Xmlns + "loc", node.Url(mode: UrlMode.Absolute)),
                    new XElement(Xmlns + "lastmod", node.UpdateDate.ToString("yyyy-MM-dd")));

                var changeFreqency = node.Value<string>("searchEngineChangeFrequency");

                if (string.IsNullOrWhiteSpace(changeFreqency) == false)
                {
                    urlElement.Add(new XElement(Xmlns + "changefreq", changeFreqency.ToLower()));
                }

                var priority = node.Value<decimal>("searchEngineRelativePriority");

                if (priority > 0)
                {
                    urlElement.Add(new XElement(Xmlns + "priority", priority));
                }

                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);

            var sitemapXml = document.ToString();

            return sitemapXml;
        }
    }
}
