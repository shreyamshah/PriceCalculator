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
            await NavigationService.NavigateAsync("MasterPage/NavigationPage/MainPage");
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
        }
    }
}
