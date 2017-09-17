using Android.App;
using Android.Content.PM;
using Android.OS;
using BarcodeReaderAbstractions;
using BarcodeReaderImpl_Droid;
using Xamarin.Forms;

namespace BarcodeScanner.Droid
{
    [Activity(Label = "BarcodeScanner", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // Perform DependencyService IBarcodeReader registration to BarcodeReaderWrapper_Droid
            DependencyService.Register<IBarcodeReader, BarcodeReaderWrapper_Droid>();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

