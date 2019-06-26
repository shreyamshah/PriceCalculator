using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class PopupPageViewModel : ViewModelBase
	{

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand ShareCommand { get; set; }
        public PopupPageViewModel(INavigationService navigationService,IPageDialogService dialogService) : base(navigationService,dialogService)
        {
            CloseCommand = new DelegateCommand(Close);
            ShareCommand = new DelegateCommand(Share);
        }

        public async void Share()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Text", Text);
            await NavigationService.GoBackAsync(parameters);
        }

        public async void Close()
        {
            await NavigationService.GoBackAsync();
        }
	}
}
