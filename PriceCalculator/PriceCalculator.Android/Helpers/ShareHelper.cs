﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class ShareHelper : Activity,IShareHelper
    {
        public ShareHelper()
        {

        }
        public void SharePicture(string imageData,string text)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraText, text);
            intent.SetType("img/jpg");
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(new Java.IO.File(imageData)));
            Android.App.Application.Context.StartActivity(Intent.CreateChooser(intent, "Share Product"));
        }
    }
}