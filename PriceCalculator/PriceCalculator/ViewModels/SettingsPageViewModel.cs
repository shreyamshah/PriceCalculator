using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class SettingsPageViewModel : ViewModelBase
	{
        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            ViewItem = new DelegateCommand(ViewItems);
            Xamarin.Forms.MessagingCenter.Subscribe<ActivityResult>(this, ActivityResult.key,OnAuthFailed);
        }

        public void OnAuthFailed(ActivityResult obj)
        {
            NavigationService.NavigateAsync("app:///MasterPage/NavigationPage/MainPage");
        }

        public DelegateCommand ViewItem { get; set; }

        public void ViewItems()
        {
            NavigationService.NavigateAsync("CategoryPage");
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            Xamarin.Forms.DependencyService.Get<IShareHelper>().Authenticate();
        }
    }
}
