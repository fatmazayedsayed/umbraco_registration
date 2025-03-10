using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.Extensions
{
    public static class PublishedContentExtensions
    {
        public static IPublishedContent? GetHomePage(this IPublishedContent content) => content.Root();

        public static IPublishedContent? GetSiteSettings(this IPublishedContent content) =>
            GetHomePage(content)?.FirstChildOfType("siteSettings");

        public static IPublishedContent? GetProductsPage(this IPublishedContent content) =>
            GetHomePage(content)?.FirstChildOfType("products");

        public static IPublishedContent? GetBlogPage(this IPublishedContent content) =>
            GetHomePage(content)?.FirstChildOfType("blog");
    }
}
