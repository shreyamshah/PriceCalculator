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
            Xamarin.Forms.MessagingCenter.Subscribe<Item>(this, "added", OnItemAdded);
            AddItemCommand = new DelegateCommand(AddItem);
            GetAllItems();
        }

        public void OnItemAdded(Item obj)
        {
            if(obj != null)
            {
                GetAllItems();
            }
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

        public void GetAllItems()
        {
            ItemsList = new ObservableCollection<Item>(App.DbHelper.GetAllItems());
        }
    }
}
