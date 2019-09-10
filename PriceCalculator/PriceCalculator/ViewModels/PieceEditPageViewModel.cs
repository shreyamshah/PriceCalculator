using Newtonsoft.Json;
using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Internals;

namespace PriceCalculator.ViewModels
{
    public class PieceEditPageViewModel : ViewModelBase
    {
        public PieceEditPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            OnImageTapped = new DelegateCommand(OpenViewer);
            AddNewItem = new DelegateCommand(AddItem);
            AddTotalCommand = new DelegateCommand<object>(AddTotal);
            RemoveItemCommand = new DelegateCommand<ItemUsed>(RemoveItem);
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
        public void OpenViewer()
        {
            if (string.IsNullOrEmpty(Product.ImgName))
            {
                if (IsFull)
                    IsFull = false;
                else
                    IsFull = true;
            }
        }

        public async void SaveProduct()
        {
            string messages = "";
            if (Product.ItemsUsed == null || (Product.ItemsUsed != null && Product.ItemsUsed.Count == 0))
            {
                messages += "Add an Item\n";
            }
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
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to save?", "Yes", "No");
            if (confirm)
            {
                int add = await App.DbHelper.SaveProduct(Product);
                foreach (var item in Product.ItemsUsed)
                {
                    if (item.Quantity > 0)
                    {
                        item.ProductId = Product.Id;
                        await App.DbHelper.SaveItemUsed(item);
                    }
                }
                await DialogService.DisplayAlertAsync("Success", "Product edited Successfully", "Ok");
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
            if (!string.IsNullOrEmpty(Product.Category))
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
            if (parameters.ContainsKey("piece"))
            {
                Product = parameters["piece"] as Product;
                SelectedCategory = CategoryList.FirstOrDefault(e => e.Name.Equals(Product.Category));
                ItemList = new List<Item>(await App.DbHelper.GetAllItems(SelectedCategory.Id.ToString()));
                /*Product.ItemsUsed = JsonConvert.DeserializeObject<ObservableCollection<ItemUsed>>(Product.Items);*/
                Product.ItemsUsed = new ObservableCollection<ItemUsed>(await App.DbHelper.GetAllItemUsed(Product.Id));
                foreach (ItemUsed item in Product.ItemsUsed)
                {
                    Item temp = ItemList.FirstOrDefault(e => e.Name.Equals(item.Type));
                    item.ItemSelected = temp;
                }
            }
        }

        public async void OnCategoryChanged(Category category)
        {
            if (category != null && Product.Category != category.Name)
            {
                ItemList = new List<Item>(await App.DbHelper.GetAllItems(category.Id.ToString()));
                Product.Category = category.Name;
                if (ItemList != null && ItemList.Count > 0)
                {
                    for (int i = Product.ItemsUsed.Count - 1; i >= 0; i--)
                    {
                        Product.ItemsUsed.RemoveAt(i);
                    }
                    foreach (Item item in ItemList)
                    {
                        if (Product.ItemsUsed.Count(e => e.Type.Equals(item.Name)) == 0)
                            Product.ItemsUsed.Add(new ItemUsed() { ItemSelected = item });
                    }
                }
            }
        }
    }
}
