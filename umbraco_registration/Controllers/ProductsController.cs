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
    public class ProductsController : RenderController
    {
        private readonly IProductSearchService _productSearchService;
        private readonly IAppPolicyCache _cache;
        public ProductsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, AppCaches caches, IProductSearchService productSearchService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _productSearchService = productSearchService;
            _cache = caches.RuntimeCache;
        }

        public IActionResult Products(int page = 1, string? keywords = null, string? category = null)
        {
            var pageSize = CurrentPage?.Value<int>("maximumPerPage") ?? 9;
            var searchCriteria = new ProductSearchCriteria
            {
                Keywords = keywords,
                Category = category,
                CurrentPage = page,
                PageSize = pageSize
            };

            var searchResults = _productSearchService.SearchProducts(searchCriteria);

            var model = new ProductsViewModel(CurrentPage)
            {
                SearchResponse = searchResults,
                Categories = GetProductCategories()
            };

            return CurrentTemplate(model);
        }

        private IEnumerable<string?> GetProductCategories()
        {
            if (CurrentPage == null)
            {
                throw new InvalidOperationException();
            }

            if (CurrentPage.GetHomePage() == null)
            {
                throw new InvalidOperationException();
            }

            return _cache.GetCacheItem("ProductCategories", () => CurrentPage.GetHomePage()!
                    .FirstChildOfType("productCategories")!
                    .Children()!
                    .Select(x => x.Value<string>("categoryName")),
                TimeSpan.FromMinutes(60)) ?? Enumerable.Empty<string?>();
        }
    }
}
