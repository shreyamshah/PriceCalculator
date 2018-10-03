using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class MasterPageViewModel : BindableBase
	{
        private INavigationService navigationService;
        public INavigationService NavigationService
        {
            get
            {
                return navigationService;
            }
            set { SetProperty(ref navigationService, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public MasterPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public void Navigate(string obj)
        {
            NavigationService.NavigateAsync(obj);
        }
    }
}
