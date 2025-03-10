using umbraco_registration.Models.Search;

namespace umbraco_registration.Interfaces
{
    public interface IProductSearchService
    {
        ProductSearchResponse SearchProducts(ProductSearchCriteria criteria);
    }
}
