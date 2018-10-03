using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace PriceCalculator.ViewModels
{
	public class PieceAddPageViewModel : ViewModelBase
	{
        public PieceAddPageViewModel(INavigationService navigationService,IPageDialogService dialogService):base(navigationService,dialogService)
        {
            OnImageTapped = new DelegateCommand(OpenViewer);
            AddNewItem = new DelegateCommand(AddItem);
            AddTotalCommand = new DelegateCommand<object>(AddTotal);
            RemoveItemCommand = new DelegateCommand<ItemUsed>(RemoveItem);
            Image = "addimage.png";
            Product = new Product();
            UnitList = new List<string>() { "kg","grams","gruss","piece" };
            ItemList = new List<Item>(App.DbHelper.GetAllItems());
        }


        #region Poperties
        public DelegateCommand OnImageTapped { get; set; }
        public DelegateCommand AddNewItem { get; set; }
        public DelegateCommand<object> AddTotalCommand { get; set; }
        public DelegateCommand<ItemUsed> RemoveItemCommand { get; set; }

        private ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        private Product product;
        public Product Product
        {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        private List<string> unitList;
        public List<string> UnitList
        {
            get { return unitList; }
            set { SetProperty(ref unitList, value); }
        }

        private List<Item> itemList;
        public List<Item> ItemList
        {
            get { return itemList; }
            set { SetProperty(ref itemList, value); }
        }

        #endregion
        public async void OpenViewer()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DialogService.DisplayAlertAsync("No Camera", " No camera available.", "OK");
                return;
            }
            IFolder folder = FileSystem.Current.LocalStorage;
            var imgFile = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "images",
                Name = Product.Name,
                CompressionQuality = 50,
                DefaultCamera = CameraDevice.Rear
            });
                if (imgFile == null)
                return;
            // create a file, overwriting any existing file  
            Image = $"{imgFile.Path}";
        }

        public void AddItem()
        {
            if (Product.ItemsUsed == null)
                Product.ItemsUsed = new ObservableCollection<ItemUsed>();
            Product.ItemsUsed.Add(new ItemUsed());
        }

        public void AddTotal(object obj)
        {
            Product.CostPrice = Product.ItemsUsed.Sum(e => e.Total);
        }

        public void RemoveItem(ItemUsed obj)
        {
            Product.ItemsUsed.Remove(obj);
        }
    }
}
