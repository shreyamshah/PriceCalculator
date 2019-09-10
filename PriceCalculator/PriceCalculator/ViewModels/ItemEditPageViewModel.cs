using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
    public class ItemEditPageViewModel : ViewModelBase
    {
        public ItemEditPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            UnitList = new List<string>() { "kg", "grams", "gross", "piece", "dozen" };
            SaveCommand = new DelegateCommand(SaveItemAsync);
            CancelCommand = new DelegateCommand(Cancel);
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
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to save?", "Yes", "No");
            if (confirm)
            {
                string messages = "";
                if (string.IsNullOrEmpty(Item.Name))
                {
                    messages += "Enter name of the Item.\n";
                }
                if (Item.Rate == 0)
                {
                    messages += "Enter a valid rate.\n";
                }
                if (string.IsNullOrEmpty(Item.Unit))
                {
                    messages += "Choose a Unit.";
                }
                if (!string.IsNullOrEmpty(messages))
                {
                    await DialogService.DisplayAlertAsync("Alert", messages, "Ok");
                    return;
                }
                await App.DbHelper.SaveItem(Item);
                //await DialogService.DisplayAlertAsync("Success", "Item edited SuccessFully", "Ok");
                Xamarin.Forms.MessagingCenter.Send<Item>(Item, "added");
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

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("item"))
            {
                Item = parameters["item"] as Item;
            }
        }
    }
}
