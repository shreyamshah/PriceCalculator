using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator.ViewModels
{
	public class CategoryAddPageViewModel : ViewModelBase
    {
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private Category category;
        public Category Category
        {
            get { return category; }
            set { SetProperty(ref category, value); }
        }
        public CategoryAddPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Category = new Category();
            SaveCommand = new DelegateCommand(SaveCategory);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public async void SaveCategory()
        {
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to save?", "Yes", "No");
            if (confirm)
            {
                string messages = "";
                if (string.IsNullOrEmpty(Category.Name))
                {
                    messages += "Name is a required field";
                }
                if (!string.IsNullOrEmpty(messages))
                {
                    await DialogService.DisplayAlertAsync("Alert", messages, "Ok");
                    return;
                }
                int add = await App.DbHelper.SaveCategory(Category);
                await DialogService.DisplayAlertAsync("Success", "Category added Successfully", "Ok");
                Xamarin.Forms.MessagingCenter.Send<Category>(Category, "added");
                await NavigationService.GoBackAsync();
            }
        }

        public async void Cancel()
        {
            bool confirm = await DialogService.DisplayAlertAsync("Confirm", "Do you want to cancel?", "Yes", "No");
            if (confirm)
            {
                await NavigationService.GoBackAsync();
            }
        }
    }
}
