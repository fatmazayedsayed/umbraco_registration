namespace umbraco_registration.Models.Search
{
    public class ProductSearchCriteria
    {
        public string? Keywords { get; set; } = null;

        public string? Category { get; set; } = null;

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 9;


    }
}
