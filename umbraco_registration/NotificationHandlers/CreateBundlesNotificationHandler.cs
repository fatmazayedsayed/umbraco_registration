using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.WebAssets;

namespace umbraco_registration.NotificationHandlers
{
    public class CreateBundlesNotificationHandler : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IRuntimeMinifier _runtimeMinifier;
        private readonly IRuntimeState _runtimeState;

        public CreateBundlesNotificationHandler(IRuntimeMinifier runtimeMinifier, IRuntimeState runtimeState)
        {
            _runtimeMinifier = runtimeMinifier;
            _runtimeState = runtimeState;
        }
        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (_runtimeState.Level == RuntimeLevel.Run)
            {
                _runtimeMinifier.CreateCssBundle("bootstrap-css-bundle",
                    BundlingOptions.OptimizedAndComposite,
                    new[] { 
                        "~/lib/bootstrap/css/bootstrap.min.css",
                        "~/lib/bootstrap-icons/font/bootstrap-icons.min.css"
                    });


                _runtimeMinifier.CreateCssBundle("custom-css-bundle",
                    BundlingOptions.OptimizedAndComposite,
                    new[] { "~/css/custom.css" });

                _runtimeMinifier.CreateJsBundle("bootstrap-js-bundle",
                    BundlingOptions.OptimizedAndComposite,
                    new[] {
                        "~/lib/bootstrap/js/bootstrap.bundle.min.js"
                    });

                _runtimeMinifier.CreateCssBundle("custom-js-bundle",
                    BundlingOptions.OptimizedAndComposite,
                    new[] { "~/scripts/custom.js" });
            }
        }
    }
}
