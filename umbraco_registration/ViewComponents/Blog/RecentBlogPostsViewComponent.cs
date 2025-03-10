using Microsoft.AspNetCore.Mvc;
using umbraco_registration.Extensions;
using umbraco_registration.ViewModels.ViewComponents;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.ViewComponents.Blog
{
    public class RecentBlogPostsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPublishedContent content, string heading = "Recent Blog Posts", string orderByAlias = "displayName")
        {
            var blogRoot = content.GetBlogPage();
            var latestBlogPosts = blogRoot?.Children()?.OrderByDescending(x => x.Value<DateTime>(orderByAlias)).Take(3);

            var model = new RecentBlogPostsViewModel
            {
                Heading = heading,
                BlogPosts = latestBlogPosts
            };

            return View("~/Views/Components/RecentBlogPosts/Default.cshtml", model);
        }
    }
}
