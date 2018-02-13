
using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Acr.UserDialogs;
using Plugin.Permissions;

namespace AssistidCollector2.Droid
{
    [Activity(Label = "Social Inclusion App",
        AlwaysRetainTaskState = true,
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Portrait,
        MainLauncher = true,
        Theme = "@style/MyTheme",
        HardwareAccelerated = true,
        MultiProcess = true,
        ConfigurationChanges = ConfigChanges.Orientation |
                               ConfigChanges.ScreenSize |
                               ConfigChanges.Keyboard |
                               ConfigChanges.KeyboardHidden)]
    [IntentFilter(new[] { Intent.ActionMain },
        Categories = new[]
        {
                Intent.CategoryHome,
                Android.Content.Intent.CategoryDefault
        })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
