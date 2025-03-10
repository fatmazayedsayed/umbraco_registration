using umbraco_registration.Interfaces;
using umbraco_registration.Services;
using umbraco_registration.Services.Search;
using Umbraco.Cms.Core.Composing;

namespace umbraco_registration.Composers
{
    public class RegisterServicesComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<IProductSearchService, ProductSearchService>();
            builder.Services.AddTransient<IBlogSearchService, BlogSearchService>();
            builder.Services.AddTransient<ISyndicationXmlService, SyndicationXmlService>();
            builder.Services.AddTransient<ISiteMapXmlService, SiteMapXmlService>();
        }
    }
}
