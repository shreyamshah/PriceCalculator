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
        public static SQLiteConnection Connection;
        public static DatabaseHelper DbHelper;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            Connection = DependencyService.Get<ISqlite>().GetConnection();
            DbHelper = new DatabaseHelper();
            List<string> sqlFiles = TableInfo.Tables;
            if(sqlFiles != null && sqlFiles.Count>0)
            {
                List<Info> sqlexecuted = new List<Info>();
                if(App.DbHelper.GetTableCount()>1)
                    sqlexecuted = App.DbHelper.GetScriptsLoaded();
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
                                    DbHelper.ExecuteQuery(query);
                                }
                            }
                            DbHelper.SaveInfo(item, "script");
                        }
                    }
                }
            }
            await NavigationService.NavigateAsync("/MasterPage/NavigationPage/MainPage");
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
        }
    }
}
