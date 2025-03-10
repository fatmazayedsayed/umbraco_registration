using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace umbraco_registration.NotificationHandlers
{
    /// <summary>
    /// After content is published
    /// </summary>
    public class OnContentDeletedNotificationHandler : INotificationHandler<ContentDeletedNotification>
    {
        private readonly AppCaches _appCaches;

        public OnContentDeletedNotificationHandler(AppCaches appCaches)
        {
            _appCaches = appCaches;
        }

        public void Handle(ContentDeletedNotification notification)
        {
            if (notification.DeletedEntities.Any(x => x.ContentType.Alias == "productCategory"))
            {
                _appCaches.RuntimeCache.ClearByKey("ProductCategories");
            }

            if (notification.DeletedEntities.Any(x => x.ContentType.Alias == "blogCategory"))
            {
                _appCaches.RuntimeCache.ClearByKey("BlogCategories");
            }
        }
    }
}