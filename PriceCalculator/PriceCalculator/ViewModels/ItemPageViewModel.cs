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
        }

        public void OnItemAdded(Item obj)
        {
            if(obj != null && category!=null)
            {
                GetAllItems(category.Id.ToString());
            }
        }

        public void AddItem()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Category", category);
            NavigationService.NavigateAsync("ItemAddPage",parameters,true,false);
        }

        public DelegateCommand AddItemCommand { get; set; }

        private ObservableCollection<Item> itemsList;
        public ObservableCollection<Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }

        private Item item;
        public Item Item
        {
            get { return item; }
            set
            {
                SetProperty(ref item, value);
                if(item != null)
                {
                    GoToEditPage(item);
                }
            }
        }

        Category category;

        public async void GetAllItems(string category)
        {
            ItemsList = new ObservableCollection<Item>(await App.DbHelper.GetAllItems(category));
        }

        public async void GoToEditPage(Item items)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("item", items);
            await NavigationService.NavigateAsync("ItemEditPage", parameters,true,false);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.ContainsKey("category"))
            {
                category = parameters["category"] as Category;
                if(category != null && category.Id.HasValue)
                {
                    GetAllItems(category.Id.ToString());
                }
            }
        }
    }
}
