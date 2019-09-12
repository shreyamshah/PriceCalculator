using Prism;
using Prism.Ioc;
using PriceCalculator.ViewModels;
using PriceCalculator.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using SQLite;
using PriceCalculator.Helper;
using PriceCalculator.Helpers;
using PriceCalculator.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Prism.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PriceCalculator
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public static SQLiteAsyncConnection Connection;
        public static DatabaseHelper DbHelper;
        public App(IPlatformInitializer initializer = null ) : base(initializer) { }
        protected override void OnInitialized()
        {
            try
            {
                InitializeComponent();
                DbHelper = new DatabaseHelper();
                GetConnection();
            }
            catch ( Exception ex)
            {
                throw ex;
            }
        }

        protected async override void OnStart()
        {
            base.OnStart();
            bool isFirst = true;
            List<string> sqlFiles = TableInfo.Tables;
            if (sqlFiles != null && sqlFiles.Count > 0)
            {
                List<Info<string>> sqlexecuted = new List<Info<string>>();
                if (await App.DbHelper.GetTableCount() > 1)
                {
                    sqlexecuted = await App.DbHelper.GetScriptsLoaded();
                    isFirst = false;
                }
                foreach (string item in sqlFiles)
                {
                    if (sqlexecuted != null && sqlexecuted.Count(e => e.key.Equals(item)) == 0)
                    {
                        string file = DependencyService.Get<IFileHelper>().GetFile(item);
                        if (!string.IsNullOrEmpty(file))
                        {
                            List<string> queries = new List<string>(file.Split(';'));
                            foreach (string query in queries)
                            {
                                if (!string.IsNullOrEmpty(query))
                                {
                                    await DbHelper.ExecuteQuery(query);
                                }
                            }
                            await DbHelper.SaveInfo(item, "script");
                        }
                    }
                    //await App.Connection.CloseAsync();
                }
            }
            if(isFirst)
            {
                await DbHelper.SaveInfo("imageCompress", bool.TrueString);
            }
            Authenticate();
            await NavigationService.NavigateAsync("LoginPage");            
            Xamarin.Forms.MessagingCenter.Subscribe<ActivityResult>(this, "success", Navigate);
        }

        public async void Navigate(ActivityResult obj)
        {
            if (obj.ResultCode.ToString() == "Ok")
            {
                string res = await DbHelper.GetInfo<string>("imageCompress");
                if (res == bool.TrueString)
                    await NavigationService.NavigateAsync("//MasterPage/NavigationPage/MainPage");
                else
                {
                    NavigationParameters parameters = new NavigationParameters();
                    parameters.Add("imageCompress", true);
                    await NavigationService.NavigateAsync("//NavigationPage/UpgradePage", parameters);
                }
            }
            else if (obj.ResultCode.ToString() == "Cancel")
                Xamarin.Forms.DependencyService.Get<ISystemHelper>().CloseApp();
        }

        public static void Authenticate()
        {
            Xamarin.Forms.DependencyService.Get<IAuthHelper>().Authenticate(1);
        }

        public static void GetConnection()
        {
            if(Connection == null)
                Connection = DependencyService.Get<ISqlite>().GetConnection();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<PieceAddPage>();
            containerRegistry.RegisterForNavigation<MasterPage>();
            containerRegistry.RegisterForNavigation<ItemAddPage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
            containerRegistry.RegisterForNavigation<ItemPage>();
            containerRegistry.RegisterForNavigation<CategoryPage>();
            containerRegistry.RegisterForNavigation<CategoryAddPage>();
            containerRegistry.RegisterForNavigation<PieceDetailPage>();
            containerRegistry.RegisterForNavigation<PartyPage>();
            containerRegistry.RegisterForNavigation<PartyAddPage, PartyAddPageViewModel>();
            containerRegistry.RegisterForNavigation<PopupPage, PopupPageViewModel>();
            containerRegistry.RegisterForNavigation<PieceEditPage, PieceEditPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemEditPage, ItemEditPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<UpgradePage, UpgradePageViewModel>();
        }
    }
}
