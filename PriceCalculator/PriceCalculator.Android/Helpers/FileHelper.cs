using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetFile(string fileName)
        {
            string fileContent = null;
            using (StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(fileName)))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
    }
}