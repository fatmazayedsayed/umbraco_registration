using umbraco_registration.NotificationHandlers;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;

namespace umbraco_registration.Composers
{
    public class RegisterNotificationHandlersComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ContentDeletedNotification, OnContentDeletedNotificationHandler>();
            builder.AddNotificationHandler<ContentMovedToRecycleBinNotification, OnContentMovedToRecycleBinNotification>();
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, CreateBundlesNotificationHandler>();
        }
    }
}
