using Umbraco.Cms.Core.Models.PublishedContent;

namespace umbraco_registration.Models.Search
{
    public class SearchResults
    {
        public IEnumerable<IPublishedContent> Items { get; set; } = Enumerable.Empty<IPublishedContent>();
        public long TotalResults { get; set; }
    }
}