using PCLStorage;
using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PriceCalculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService,IPageDialogService dialogService) 
            : base (navigationService,dialogService)
        {
            Title = "Main Page";
            AddCommand = new DelegateCommand(Add);
            GetAllProducts();
            Xamarin.Forms.MessagingCenter.Subscribe<Product>(this, "added", OnProductAdded);
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

        #endregion

        public async void Add()
        {
            await NavigationService.NavigateAsync("PieceAddPage");
        }

        public void GetAllProducts()
        {
            ProductsList = new ObservableCollection<Product>(App.DbHelper.GetAllProducts());
        }

        public void OnProductAdded(Product obj)
        {
            if(obj != null)
            {
                GetAllProducts();
            }
        }

    }
}
