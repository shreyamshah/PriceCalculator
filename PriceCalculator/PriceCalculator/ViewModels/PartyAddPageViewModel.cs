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
	public class PartyAddPageViewModel : ViewModelBase
	{
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private Party party;
        public Party Party
        {
            get { return party; }
            set { SetProperty(ref party, value); }
        }

        public PartyAddPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Party = new Party();
            SaveCommand = new DelegateCommand(SaveParty);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public async void SaveParty()
        {
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to save?", "Yes", "No");
            if (confirm)
            {
                string messages = "";
                if (string.IsNullOrEmpty(Party.Name))
                {
                    messages += "Name is a required field";
                }
                if (Party.ProfitPercent == 0)
                {
                    messages += "Profit Percent is a required field";
                }
                if (!string.IsNullOrEmpty(messages))
                {
                    await DialogService.DisplayAlertAsync("Alert", messages, "Ok");
                    return;
                }
                int add = await App.DbHelper.SaveParty(Party);
                await DialogService.DisplayAlertAsync("Success", "Party added Successfully", "Ok");
                Xamarin.Forms.MessagingCenter.Send<Party>(Party, "added");
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
    }
}
