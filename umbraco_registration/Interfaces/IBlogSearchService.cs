using umbraco_registration.Models.Search;

namespace umbraco_registration.Interfaces
{
    public interface IBlogSearchService
    {
        BlogSearchResponse SearchProducts(BlogSearchCriteria criteria);
    }
}
