using Examine;
using Examine.Search;
using umbraco_registration.Interfaces;
using umbraco_registration.Models.Search;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Web.Common;

namespace umbraco_registration.Services.Search
{
    internal class BlogSearchService : IBlogSearchService
    {
        private readonly IExamineManager _examineManager;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IShortStringHelper _shortStringHelper;

        public BlogSearchService(IExamineManager examineManager, UmbracoHelper umbracoHelper, IShortStringHelper shortStringHelper)
        {
            _examineManager = examineManager;
            _umbracoHelper = umbracoHelper;
            _shortStringHelper = shortStringHelper;
        }

        public BlogSearchResponse SearchProducts(BlogSearchCriteria criteria)
        {
            var response = new BlogSearchResponse(criteria);

            response.SearchResults = SearchUsingExamine(criteria);

            return response;
        }

        private SearchResults SearchUsingExamine(BlogSearchCriteria criteria)
        {
            var results = new SearchResults();
            if (_examineManager.TryGetIndex(Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                var fieldsToSearch = new[]
                {
                    "nodeName",
                    "categoryNames",
                    "description",
                    "categories",
                    "detailedDescription"
                };

                var query = index.Searcher.CreateQuery("content").NodeTypeAlias("blogPost");

                if (!string.IsNullOrEmpty(criteria.Keywords))
                {
                    query.And().GroupedOr(fieldsToSearch, criteria.Keywords.MultipleCharacterWildcard());
                }

                if (!string.IsNullOrEmpty(criteria.Category))
                {
                    query.And().Field("categoryNames", criteria.Category.ToUrlSegment(_shortStringHelper));
                }

                var searchResults = new List<IPublishedContent>();
                var stringToParse = query.ToString();
                var indexOfPropertyValue = stringToParse?.IndexOf("LuceneQuery:") + 12;
                if (indexOfPropertyValue.HasValue)
                {
                    var rawQuery = stringToParse?.Substring(indexOfPropertyValue.Value).TrimEnd('}');
                    var response = index.Searcher.CreateQuery("content").NativeQuery(rawQuery).Execute(QueryOptions.SkipTake((criteria.CurrentPage - 1) * criteria.PageSize, criteria.PageSize));
                    foreach (var id in response.Select(x => x.Id))
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            var contentItem = _umbracoHelper.Content(id);

                            if (contentItem != null)
                            {
                                searchResults.Add(contentItem);
                            }
                        }
                    }
                    results.Items = searchResults;
                    results.TotalResults = response.TotalItemCount;
                }
            }

            return results;
        }
    }
}
