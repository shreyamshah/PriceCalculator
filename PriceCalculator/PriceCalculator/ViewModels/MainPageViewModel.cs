using PCLStorage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceCalculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService,IPageDialogService dialogService) 
            : base (navigationService,dialogService)
        {
            Title = "Main Page";
            AddCommand = new DelegateCommand(Add);
        }

        #region Property
        public DelegateCommand AddCommand { get; set; }
        #endregion

        public async void Add()
        {
            await NavigationService.NavigateAsync("PieceAddPage");
        }
    }
}
