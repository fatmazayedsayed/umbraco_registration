using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using umbraco_registration.Extensions;
using umbraco_registration.Interfaces;
using umbraco_registration.Models.Search;
using umbraco_registration.ViewModels;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace umbraco_registration.Controllers
{
    public class BlogController : RenderController
    {
        private readonly IBlogSearchService _blogSearchService;
        private readonly IAppPolicyCache _cache;
        public BlogController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, AppCaches caches, IBlogSearchService blogSearchService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _blogSearchService = blogSearchService;
            _cache = caches.RuntimeCache;
        }

        public IActionResult Blog(int page = 1, string? keywords = null, string? category = null)
        {
            var pageSize = CurrentPage?.Value<int>("maximumPerPage") ?? 9;
            var searchCriteria = new BlogSearchCriteria
            {
                Keywords = keywords,
                Category = category,
                CurrentPage = page,
                PageSize = pageSize
            };

            var searchResults = _blogSearchService.SearchProducts(searchCriteria);

            var model = new BlogViewModel(CurrentPage)
            {
                SearchResponse = searchResults,
                Categories = GetBlogCategories()
            };

            return CurrentTemplate(model);
        }

        private IEnumerable<string?> GetBlogCategories()
        {
            if (CurrentPage == null)
            {
                throw new InvalidOperationException();
            }

            if (CurrentPage.GetHomePage() == null)
            {
                throw new InvalidOperationException();
            }

            return _cache.GetCacheItem("BlogCategories", () => CurrentPage.GetHomePage()!
                    .FirstChildOfType("blogCategories")!
                    .Children()!
                    .Select(x => x.Value<string>("categoryName")),
                TimeSpan.FromMinutes(60)) ?? Enumerable.Empty<string?>();
        }
    }
}
