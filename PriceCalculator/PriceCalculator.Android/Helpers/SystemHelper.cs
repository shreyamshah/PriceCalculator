using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(SystemHelper))]
namespace PriceCalculator.Droid.Helpers
{
    class SystemHelper : ISystemHelper
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}