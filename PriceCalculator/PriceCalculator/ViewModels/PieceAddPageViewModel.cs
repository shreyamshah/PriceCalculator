using Newtonsoft.Json;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Stormlion.ImageCropper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PriceCalculator.ViewModels
{
    public class PieceAddPageViewModel : ViewModelBase
    {
        public PieceAddPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            OnImageTapped = new DelegateCommand(OpenViewer);
            AddNewItem = new DelegateCommand(AddItem);
            AddTotalCommand = new DelegateCommand<object>(AddTotal);
            RemoveItemCommand = new DelegateCommand<ItemUsed>(RemoveItem);
            //ShareImage = new DelegateCommand(Share);
            Image = "addimage.png";
            Product = new Product();
            UnitList = new List<string>() { "kg", "grams", "gross", "piece" };
            SaveCommand = new DelegateCommand(SaveProduct);
            CancelCommand = new DelegateCommand(Cancel);    
            Width = Hieght = 30;
            IsFull = false;
        }


        #region Poperties
        public DelegateCommand OnImageTapped { get; set; }
        public DelegateCommand AddNewItem { get; set; }
        public DelegateCommand<object> AddTotalCommand { get; set; }
        public DelegateCommand<ItemUsed> RemoveItemCommand { get; set; }
        public DelegateCommand ShareImage { get; set; }

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
        public static Action<string,string> Success { get; set; }

        public List<Item> ItemList
        {
            get { return itemList; }
            set { SetProperty(ref itemList, value); }
        }

        private List<Category> categoryList;
        public List<Category> CategoryList
        {
            get { return categoryList; }
            set { SetProperty(ref categoryList, value); }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                SetProperty(ref selectedCategory, value);
                OnCategoryChanged(selectedCategory);
            }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }

        private int hieght;
        public int Hieght
        {
            get { return hieght; }
            set { SetProperty(ref hieght, value); }
        }

        private bool isFull;
        public bool IsFull
        {
            get { return isFull; }
            set { SetProperty(ref isFull, value); }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        #endregion
        public async void OpenViewer()
        {
            if (string.IsNullOrEmpty(Product.ImgName))
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DialogService.DisplayAlertAsync("No Camera", " No camera available.", "OK");
                    return;
                }
                var imgFile = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    //Name = DateTime.Now.ToString(),
                    DefaultCamera = CameraDevice.Rear
                });
                if (imgFile == null)
                    return;
                // create a file, overwriting any existing file  
                Product.ImgName = imgFile.Path;
                //Width = Hieght = 150;
                Image = $"{imgFile.Path}";
                Xamarin.Forms.DependencyService.Get<IImageCropHelper>().ShowFromFile(imgFile.Path);
            }
            else if (IsFull)
                IsFull = false;
            else
                IsFull = true;
            Success = (newFile,oldFile) =>
              {
                  Xamarin.Forms.DependencyService.Get<IFileHelper>().DeleteFile(oldFile);
                  Xamarin.Forms.DependencyService.Get<IFileHelper>().RenameFile(oldFile, newFile);
                  Image = oldFile;
              };
        }

        public async void SaveProduct()
        {
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to save?", "Yes", "No");
            if (confirm)
            {
                string messages = "";
                if (string.IsNullOrEmpty(Product.ImgName))
                {
                    messages += "Choose a Product Image\n";
                }
                if (Product.ItemsUsed == null || (Product.ItemsUsed != null && Product.ItemsUsed.Count == 0))
                {
                    messages += "Add an Item\n";
                }
                //else if(Product.ItemsUsed.Count(e=>e.Quantity==0 || string.IsNullOrEmpty(e.Type))>0)
                //{
                //messages += "Enter 'Quantity' and 'Type' of every Item\n";
                //}
                if (Product.ProfitPercent == 0)
                {
                    messages += "Enter the Profit %\n";
                }
                if (string.IsNullOrEmpty(Product.Name))
                {
                    messages += "Enter the Name of the Product";
                }
                if (!string.IsNullOrEmpty(messages))
                {
                    await DialogService.DisplayAlertAsync("Alert", messages, "Ok");
                    return;
                }
                //Product.Items = JsonConvert.SerializeObject(Product.ItemsUsed);
                int add = await App.DbHelper.SaveProduct(Product);
                foreach (var item in Product.ItemsUsed)
                {
                    if(item.Quantity>0)
                    {
                        item.ProductId = Product.Id;
                        await App.DbHelper.SaveItemUsed(item);
                    }
                }
                await DialogService.DisplayAlertAsync("Success", "Product added Successfully", "Ok");
                Xamarin.Forms.MessagingCenter.Send<Product>(Product, "added");
                await NavigationService.GoBackAsync();
            }
        }

        public async void Cancel()
        {
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to cancel?", "Yes", "No");
            if (confirm)
            {
                await NavigationService.GoBackAsync();
            }
        }

        public async void AddItem()
        {
            if(!string.IsNullOrEmpty(Product.Category))
            {
                if (Product.ItemsUsed == null)
                    Product.ItemsUsed = new ObservableCollection<ItemUsed>();
                Product.ItemsUsed.Add(new ItemUsed());
            }
            else
            {
                await DialogService.DisplayAlertAsync("Alert", "Select Category before adding an item", "Ok");
            }
        }

        public void AddTotal(object obj)
        {
            Product.CostPrice = Product.ItemsUsed.Sum(e => e.Total);
        }

        public void RemoveItem(ItemUsed obj)
        {
            Product.ItemsUsed.Remove(obj);
            if (Product.ItemsUsed != null && Product.ItemsUsed.Count > 0)
            {
                AddTotal(obj);
            }
            else
                Product.CostPrice = 0;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            CategoryList = await App.DbHelper.GetAllCategory();
        }

        public async void OnCategoryChanged(Category category)
        {
            if (category != null)
            {
                Product.Category = category.Name;
                ItemList = new List<Item>(await App.DbHelper.GetAllItems(category.Id.ToString()));
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (Item item in ItemList)
                    {
                        Product.ItemsUsed.Add(new ItemUsed() { ItemSelected = item });
                    }
                }
            }
        }
    }
}
