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
        public DelegateCommand SaveCategoryCommand { get; set; }

        private Category category;
        public Category Category
        {
            get { return category; }
            set { SetProperty(ref category, value); }
        }
        public CategoryAddPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Category = new Category();
            SaveCategoryCommand = new DelegateCommand(SaveCategory);
        }

        public async void SaveCategory()
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
            int add = App.DbHelper.SaveCategory(Category);
            await DialogService.DisplayAlertAsync("Success", "Category added Successfully", "Ok");
            Xamarin.Forms.MessagingCenter.Send<Category>(Category, "added");
            await NavigationService.GoBackAsync();
        }
	}
}
