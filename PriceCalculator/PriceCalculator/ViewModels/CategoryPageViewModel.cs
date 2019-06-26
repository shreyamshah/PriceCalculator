using PriceCalculator.Data;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PriceCalculator.ViewModels
{
	public class CategoryPageViewModel : ViewModelBase
	{
        public DelegateCommand AddCategoryCommand { get; set; }
        
        public CategoryPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Xamarin.Forms.MessagingCenter.Subscribe<Category>(this, "added", OnCategoryAdded);
            AddCategoryCommand = new DelegateCommand(AddCategory);
            //GetAllCategory();
        }


        public void OnCategoryAdded(Category obj)
        {
            if (obj != null)
            {
                GetAllCategory();
            }
        }

        public void GoToItem(Category obj)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("category", obj as Category);
            Category = null;
            NavigationService.NavigateAsync("ItemPage", parameters);
        }

        public void AddCategory()
        {
            NavigationService.NavigateAsync("CategoryAddPage",null,true,true);
        }

        public DelegateCommand AddItemCommand { get; set; }
        private ObservableCollection<Category> categoryList;
        public ObservableCollection<Category> CategoryList
        {
            get { return categoryList; }
            set { SetProperty(ref categoryList, value); }
        }

        private Category category;
        public Category Category
        {
            get { return category; }
            set
            {
                SetProperty(ref category, value);
                if (category != null)
                    GoToItem(category);
            }
        }

        public async void GetAllCategory()
        {
            CategoryList = new ObservableCollection<Category>(await App.DbHelper.GetAllCategory());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetAllCategory();
        }
    }
}
