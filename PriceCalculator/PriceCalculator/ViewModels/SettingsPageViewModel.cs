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
        }
        public DelegateCommand ViewItem { get; set; }

        public void ViewItems()
        {
            NavigationService.NavigateAsync("ItemPage");
        }
	}
}
