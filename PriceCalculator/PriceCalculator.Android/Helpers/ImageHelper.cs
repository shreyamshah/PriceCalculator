using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Plugin.CurrentActivity;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageHelper))]
namespace PriceCalculator.Droid.Helpers
{
    class ImageHelper : IImageHelper
    {
        public async Task<string> ResizeImage(string imageArray)
        {
            try
            {
                string environmentPath = CrossCurrentActivity.Current.AppContext.ApplicationContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                if (Directory.Exists(environmentPath))
                {
                    Bitmap originalImage = await BitmapFactory.DecodeFileAsync(environmentPath +"/"+ imageArray);
                    //ImageSource source = null;
                    using (System.IO.MemoryStream stream = new MemoryStream())
                    {
                        stream.Position = 0;
                        originalImage.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                        byte[] byteArray = stream.GetBuffer();
                        string path = environmentPath + "/../Compress/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                            Java.IO.File file = new Java.IO.File(path + ".nomedia");
                            file.CreateNewFile();
                        }
                        using (var newFile = new Java.IO.File(path + imageArray))
                        {
                            newFile.CreateNewFile();
                            await System.IO.File.WriteAllBytesAsync(newFile.Path,byteArray);
                        }
                    }
                    return imageArray;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }
        public string GetCompressImagePath()
        {
            string environmentPath = CrossCurrentActivity.Current.AppContext.ApplicationContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            if (Directory.Exists(environmentPath))
            {
                string path = environmentPath + "/../Compress/";
                if (Directory.Exists(path))
                {
                    return path;
                }
            }
            return null;
        }
    }
}