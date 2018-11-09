using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class MasterPageViewModel : ViewModelBase
	{
        public DelegateCommand<string> NavigateCommand { get; set; }
        public MasterPageViewModel(INavigationService navigationService,IPageDialogService dialogSerivce) :base(navigationService,dialogSerivce)
        {
            NavigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public void Navigate(string obj)
        {
            NavigationService.NavigateAsync(obj);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
