
using Foundation;
using UIKit;

namespace AssistidCollector2.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    
        /// <summary>
        /// Gets the supported interface orientations.
        /// </summary>
        /// <returns>The supported interface orientations.</returns>
        /// <param name="application">Application.</param>
        /// <param name="forWindow">For window.</param>
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }
    }
}
