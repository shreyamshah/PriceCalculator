using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Plugin.CurrentActivity;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(PictureHelper))]
namespace PriceCalculator.Droid.Helpers
{
    class PictureHelper : IPictureHelper
    {
        public async void SavePicture(string filename, ImageSource imageData)
        {
            try
            {
                var renderer = new StreamImagesourceHandler();
                var photo = await renderer.LoadImageAsync(imageData, CrossCurrentActivity.Current.Activity);
                var documentsDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string pngFilename = System.IO.Path.Combine(documentsDirectory, ".jpeg");
                using (FileStream fs = new FileStream(documentsDirectory, FileMode.OpenOrCreate))
                {
                    photo.Compress(Bitmap.CompressFormat.Jpeg, 100, fs);
                }
            }
            catch (Exception ex)
            {
                string exMessageString = ex.Message;
            }
        }
    }
}