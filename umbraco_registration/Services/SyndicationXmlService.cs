using System.ServiceModel.Syndication;
using UmbCheckout.Shared.Extensions;
using umbraco_registration.Extensions;
using umbraco_registration.Interfaces;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace umbraco_registration.Services
{
    internal class SyndicationXmlService : ISyndicationXmlService
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        public SyndicationXmlService(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }

        public Rss20FeedFormatter GenerateRssXml(string feedTitle, string feedDescription)
        {
            return new Rss20FeedFormatter(GenerateSyndicationFeed(feedTitle, feedDescription));
        }

        public Atom10FeedFormatter GenerateAtomXml(string feedTitle, string feedDescription)
        {

            return new Atom10FeedFormatter(GenerateSyndicationFeed(feedTitle, feedDescription));
        }

        private SyndicationFeed GenerateSyndicationFeed(string feedTitle, string feedDescription)
        {
            using var context = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            var rootNode = context.Content?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == "home");
            var feed = new SyndicationFeed(feedTitle, feedDescription,
                new Uri(context.CleanedUmbracoUrl.GetLeftPart(UriPartial.Authority)),
                context.OriginalRequestUrl.AbsoluteUri,
                DateTime.Now)
            {
                Copyright = new TextSyndicationContent($"{DateTime.Now.Year} {rootNode?.GetSiteSettings()?.Value<string>("siteName")}")
            };

            if (rootNode != null)
            {
                var nodes = rootNode
                    .GetBlogPage()?.Children?
                    .Where(x => x.HasTemplate())
                    .Where(x => x.IsVisible())
                    .Where(x => x.Value<bool>("hideFromXmlSitemap") == false)
                    .OrderByDescending(x => x.Value<DateTime>("displayDate")).ToList();

                var items = new List<SyndicationItem>();

                if (nodes != null && nodes.Any())
                {
                    foreach (var node in nodes)
                    {
                        var title = node.HasValue("heading") ? node.Value<string>("heading") : node.Name;
                        var description = new TextSyndicationContent(node.Value<string>("excerpt"));
                        items.Add(
                            new SyndicationItem(
                                title,
                                description,
                                new Uri(node.Url(null, UrlMode.Absolute)),
                                node.UrlSegment(),
                                node.Value<DateTime>("displayDate")));
                    }
                    feed.Items = items;
                }
            }

            return feed;
        }
    }
}