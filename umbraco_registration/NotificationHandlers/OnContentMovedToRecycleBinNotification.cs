using Microsoft.Extensions.Caching.Memory;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace umbraco_registration.NotificationHandlers
{
    /// <summary>
    /// After content is published
    /// </summary>
    public class OnContentMovedToRecycleBinNotification : INotificationHandler<ContentMovedToRecycleBinNotification>
    {
        private readonly AppCaches _appCaches;

        public OnContentMovedToRecycleBinNotification(AppCaches appCaches)
        {
            _appCaches = appCaches;
        }

        public void Handle(ContentMovedToRecycleBinNotification notification)
        {

            var contentItems = notification.MoveInfoCollection.Select(x => x.Entity);
            if (contentItems.Any(x => x.ContentType.Alias == "productCategory"))
            {
                _appCaches.RuntimeCache.ClearByKey("ProductCategories");
            }

            if (contentItems.Any(x => x.ContentType.Alias == "blogCategory"))
            {
                _appCaches.RuntimeCache.ClearByKey("BlogCategories");
            }
        }
    }
}