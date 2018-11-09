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
                GetAllItems(category.Name);
            }
        }

        public void AddItem()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Category", category);
            NavigationService.NavigateAsync("ItemAddPage",parameters);
        }

        public DelegateCommand AddItemCommand { get; set; }
        private ObservableCollection<Item> itemsList;
        public ObservableCollection<Item> ItemsList
        {
            get { return itemsList; }
            set { SetProperty(ref itemsList, value); }
        }

        Category category;

        public void GetAllItems(string category)
        {
            ItemsList = new ObservableCollection<Item>(App.DbHelper.GetAllItems(category));
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
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
