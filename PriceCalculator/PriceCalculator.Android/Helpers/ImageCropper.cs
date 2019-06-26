using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using static Com.Theartofdev.Edmodo.Cropper.CropImage;
using static Com.Theartofdev.Edmodo.Cropper.CropImageView;

[assembly: Xamarin.Forms.Dependency(typeof(ImageCropHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class ImageCropHelper : IImageCropHelper
    {
        public ImageCropHelper()
        {
        }
        public void ShowFromFile(string imageFile)
        {
            ActivityBuilder activityBuilder = Activity(Android.Net.Uri.FromFile(new Java.IO.File(imageFile)));
            activityBuilder.SetCropShape(CropShape.Rectangle);
            activityBuilder.SetFixAspectRatio(false);
            activityBuilder.Start(CrossCurrentActivity.Current.Activity);
        }
    }
}