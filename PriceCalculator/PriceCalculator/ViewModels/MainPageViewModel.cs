using PCLStorage;
using PriceCalculator.Data;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService,IPageDialogService dialogService) 
            : base (navigationService,dialogService)
        {
            Title = "Main Page";
            AddCommand = new DelegateCommand(Add);
            Xamarin.Forms.MessagingCenter.Subscribe<Product>(this, "added", OnProductAdded);
            //Position = 0;
            SearchProductCommand = new DelegateCommand(SearchProduct);
        }

        #region Property
        public DelegateCommand AddCommand { get; set; }

        private ObservableCollection<Product> productsList;
        public ObservableCollection<Product> ProductsList
        {
            get { return productsList; }
            set { SetProperty(ref productsList, value); }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                SetProperty(ref selectedProduct, value);
                if(selectedProduct != null)
                {
                    NavigationParameters parameter = new NavigationParameters();
                    parameter.Add("Product", selectedProduct);
                    NavigationService.NavigateAsync("PieceDetailPage", parameter);
                }
            }
        }

        private List<string> categories;
        public List<string> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                SetProperty(ref position, value);
                if (Categories != null && Categories.Count > 0)
                    GetAllProducts(Categories[position]);
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);
                if(string.IsNullOrEmpty(searchText) && tempList != null && tempList.Count>0)
                {
                    ProductsList = tempList;
                }
            }
        }
        private ObservableCollection<Product> tempList;
        public DelegateCommand SearchProductCommand { get; set; }

        #endregion

        public async void Add()
        {
            await NavigationService.NavigateAsync("PieceAddPage",null,true,true);
        }

        public async void GetAllProducts(string category)
        {
            ProductsList = new ObservableCollection<Product>(await App.DbHelper.GetAllProducts(category));
            tempList = ProductsList;
        }

        public void OnProductAdded(Product obj)
        {
            if(obj != null)
            {
                if(Categories != null && Categories.Count>0)
                    GetAllProducts(Categories[Position]);
            }
        }

        public void SearchProduct()
        {
                ProductsList = new ObservableCollection<Product>(tempList.Where(e => e.Name.ToLower().Contains(SearchText.ToLower())));
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await App.DbHelper.GetAllCategory();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            List<Category> category = await GetAllCategories();
            if (category != null && category.Count > 0)
                Categories = category.Select(e => e.Name).ToList();
            else
                Categories = new List<string>();
            Position = 0;
        }
    }
}
