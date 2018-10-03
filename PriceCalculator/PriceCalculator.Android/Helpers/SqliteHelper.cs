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
using PriceCalculator.Helpers;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteHelper))]
namespace PriceCalculator.Droid.Helpers
{
    public class SqliteHelper : ISqlite
    {
        public SqliteHelper()
        {
        }

        public SQLiteConnection GetConnection()
        {
            try
            {
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal, System.Environment.SpecialFolderOption.Create), "expenseManager.sqlite");
                if (!File.Exists(path))
                {
                    using (var asset = Android.App.Application.Context.Assets.Open("priceCalculator.sqlite"))
                    using (var dest = File.Create(path))
                        asset.CopyTo(dest);
                    Console.WriteLine("Database Path:" + Path.GetFullPath(path));
                    return new SQLiteConnection(path);
                }
                return new SQLiteConnection(path);
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
