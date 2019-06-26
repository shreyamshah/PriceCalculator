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
using Plugin.CurrentActivity;
using PriceCalculator.Droid.Helpers;
using PriceCalculator.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        public AuthHelper()
        {
            
        }

        public void Authenticate(int requestCode = 0)
        {
            try
            {

                KeyguardManager manager = (KeyguardManager)Android.App.Application.Context.GetSystemService(Context.KeyguardService);
                if (manager.IsKeyguardSecure)
                {
                    var intent = manager.CreateConfirmDeviceCredentialIntent("Enter Password", "Enter your password to authenticate yourself");
                    intent.SetType(null);
                    if (intent != null)
                    {
                        //Android.App.Application.Context.StartActivity(intent);
                        CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, requestCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}