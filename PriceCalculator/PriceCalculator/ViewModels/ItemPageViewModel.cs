using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class ItemPageViewModel : ViewModelBase
	{
        public ItemPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            ItemsList = new ObservableCollection<Item>(App.DbHelper.GetAllItems());
            AddItemCommand = new DelegateCommand(AddItem);
        }

        public void AddItem()
        {
            NavigationService.NavigateAsync("ItemAddPage");
        }

        public DelegateCommand AddItemCommand { get; set; }
        private ObservableCollection<Item> itemsList;
        public ObservableCollection<Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }
    }
}
