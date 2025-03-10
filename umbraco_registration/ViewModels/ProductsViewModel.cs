using umbraco_registration.Models.Search;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.ViewModels
{
    public class ProductsViewModel : ContentModel
    {
        public ProductsViewModel(IPublishedContent? content) : base(content)
        {
        }

        public IEnumerable<string?> Categories { get; set; } = Enumerable.Empty<string?>();
        public ProductSearchResponse? SearchResponse { get; set; }
    }
}
