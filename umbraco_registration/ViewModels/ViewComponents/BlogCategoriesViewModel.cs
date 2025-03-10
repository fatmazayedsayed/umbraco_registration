namespace umbraco_registration.ViewModels.ViewComponents
{
    public class BlogCategoriesViewModel
    {
        public string Heading { get; set; } = "Categories";

        public IEnumerable<string?> Categories { get; set; } = Enumerable.Empty<string>();
    }
}
