using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceCalculator.ViewModels
{
	public class ItemAddPageViewModel : ViewModelBase
	{
        public ItemAddPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            UnitList = new List<string>() { "kg", "grams", "gross", "piece","dozen" };
            Item = new Item();
            SaveItem = new DelegateCommand(SaveItemAsync);
        }

        private List<string> unitList;
        public List<string> UnitList
        {
            get { return unitList; }
            set { SetProperty(ref unitList, value); }
        }

        private Item item;
        public Item Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public async void SaveItemAsync()
        {
            string messages = "";
            if(string.IsNullOrEmpty(Item.Name))
            {
                messages += "Enter name of the Item.\n";
            }
            if(Item.Rate==0)
            {
                messages += "Enter a valid rate.\n";
            }
            if(string.IsNullOrEmpty(Item.Unit))
            {
                messages += "Choose a Unit.";
            }
            if(!string.IsNullOrEmpty(messages))
            {
                await DialogService.DisplayAlertAsync("Alert", messages, "Ok");
                return;
            }
            App.DbHelper.SaveItem(Item);
            await DialogService.DisplayAlertAsync("Success", "Item saved SuccessFully", "Ok");
            Xamarin.Forms.MessagingCenter.Send<Item>(Item, "added");
            await NavigationService.GoBackAsync();
        }

        public DelegateCommand SaveItem { get; set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.ContainsKey("Category"))
            {
                Category category = parameters["Category"] as Category;
                if(category!=null && category.Id.HasValue)
                {
                    Item.CategoryId = category.Id.ToString();
                }
            }
        }
    }
}
