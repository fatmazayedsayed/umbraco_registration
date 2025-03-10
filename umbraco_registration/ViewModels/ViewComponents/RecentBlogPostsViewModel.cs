using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.ViewModels.ViewComponents
{
    public class RecentBlogPostsViewModel
    {
        public string Heading { get; set; } = "Recent Blog Posts";
        public IEnumerable<IPublishedContent>? BlogPosts { get; set; } = Enumerable.Empty<IPublishedContent>();
    }
}
