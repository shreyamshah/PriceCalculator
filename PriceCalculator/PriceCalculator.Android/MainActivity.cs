using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Com.Theartofdev.Edmodo.Cropper;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using PriceCalculator.ViewModels;
using Prism;
using Prism.Ioc;

namespace PriceCalculator.Droid
{
    [Activity(Label = "Price Calculator", Icon = "@mipmap/ic_launcher", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.ActionBar.SetIcon(null);
            CrossCurrentActivity.Current.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Fabric.Fabric.With(this, new Crashlytics.Crashlytics());
            Stormlion.ImageCropper.Droid.Platform.Init();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
                if (resultCode == Result.Canceled)
                {
                    Xamarin.Forms.MessagingCenter.Send(new ActivityResult { RequestCode = (int)resultCode, ResultCode = resultCode, Data = data }, ActivityResult.key);
                }
            }
            else if(requestCode == 1)
            {
                Xamarin.Forms.MessagingCenter.Send(new ActivityResult { RequestCode = (int)resultCode, ResultCode = resultCode, Data = data }, "success");
            }
            else
            {
                if (resultCode != Result.Canceled)
                {
                    CropImage.ActivityResult result = CropImage.GetActivityResult(data);
                    PieceAddPageViewModel.Success?.Invoke(result.Uri.Path, result.OriginalUri.Path);
                }
            }
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

