namespace umbraco_registration.Extensions
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddSettings(this IUmbracoBuilder builder)
        {
            builder.Services.Configure<Settings>(builder.Config.GetSection("SiteSettings"));
            return builder;
        }
    }
}
