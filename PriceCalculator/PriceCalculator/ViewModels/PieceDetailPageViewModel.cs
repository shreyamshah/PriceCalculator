using Newtonsoft.Json;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class PieceDetailPageViewModel : ViewModelBase
	{
        public PieceDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            OnImageTapped = new DelegateCommand(OpenViewer);
            ShareImage = new DelegateCommand(Share);
            IsFull = false;
        }

        public DelegateCommand OnImageTapped { get; set; }
        public DelegateCommand ShareImage { get; set; }

        private bool isFull;
        public bool IsFull
        {
            get { return isFull; }
            set { SetProperty(ref isFull, value); }
        }

        private Product product;
        public Product Product
        {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        public void OpenViewer()
        {
            if (IsFull)
                IsFull = false;
            else
                IsFull = true;
        }

        public void Share()
        {
            if(Product!=null && !string.IsNullOrEmpty(Product.ImgName) && !string.IsNullOrEmpty(Product.Name))
            {
                Xamarin.Forms.DependencyService.Get<IShareHelper>().SharePicture(Product.ImgName, Product.SellingPrice, Product.Name);
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.ContainsKey("Product"))
            {
                Product = parameters["Product"] as Product;
                try
                {
                    Product.ItemsUsed = new ObservableCollection<ItemUsed>(JsonConvert.DeserializeObject<List<ItemUsed>>(Product.Items));
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
