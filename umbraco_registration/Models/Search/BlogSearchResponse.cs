using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.Models.Search
{
    public class BlogSearchResponse
    {
        public BlogSearchResponse(BlogSearchCriteria criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        public BlogSearchCriteria Criteria { get; private set; }

        public SearchResults? SearchResults { get; set; }
    }
}
