using Plugin.FilePicker.Abstractions;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using PriceCalculator.Data;
using PriceCalculator.Helper;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceCalculator.ViewModels
{
	public class SettingsPageViewModel : ViewModelBase
	{
        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            ViewItem = new DelegateCommand(ViewItems);
            ViewParty = new DelegateCommand(ViewParties);
            Xamarin.Forms.MessagingCenter.Subscribe<ActivityResult>(this, ActivityResult.key,OnAuthFailed);
            BackupDbCommand = new DelegateCommand(BackupDbAsync);
            RestoreDbCommand = new DelegateCommand(RestoreDb);
            IsBusy = false;
        }

        public void OnAuthFailed(ActivityResult obj)
        {
            NavigationService.NavigateAsync("app:///MasterPage/NavigationPage/MainPage");
        }

        public DelegateCommand ViewItem { get; set; }
        public DelegateCommand ViewParty { get; set; }
        public DelegateCommand BackupDbCommand { get; set; }
        public DelegateCommand RestoreDbCommand { get; set; }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }


        public void ViewItems()
        {
            NavigationService.NavigateAsync("CategoryPage");
        }

        public void ViewParties()
        {
            NavigationService.NavigateAsync("PartyPage");
        }

        public async void BackupDbAsync()
        {
            IsBusy = true;
            await Task.Delay(500);
            bool res = await Xamarin.Forms.DependencyService.Get<IFileHelper>().SaveZipToFolder();
            if(res)
            {
                await DialogService.DisplayAlertAsync("Success", "Data backup sucessfull", "Ok");
            }
            else

                await DialogService.DisplayAlertAsync("Alert", "Data backup not sucessfull", "Ok");
            IsBusy = false;
            App.DbHelper.GetAllParty();
        }

        public async void RestoreDb()
        {
            bool res = await DialogService.DisplayAlertAsync("Confirm","Restoring the previous data will replace all your current data.\n Do you wish to continue?", "Yes", "No");
            if (res)
            {
                FileData file = null;
                try
                {
                    file = await Plugin.FilePicker.CrossFilePicker.Current.PickFile(new string[] { "application/zip" });
                }
                catch (Exception ex)
                {

                    throw;
                }
                if (file == null)
                    return;
                //await Task.Delay(500);
                IsBusy = true;
                List<byte> dataArray = new List<byte>(file.DataArray);
                //file.DataArray.CopyTo(dataArray, 0);
                bool restore = await Xamarin.Forms.DependencyService.Get<IFileHelper>().UnzipDb(dataArray.ToArray());
                IsBusy = false;
                if(restore)
                {
                    await DialogService.DisplayAlertAsync("Success", "Data restored successfully", "Ok");
                }
                else
                {
                    await DialogService.DisplayAlertAsync("Alert", "There is some issue in restoring", "Ok");
                }
                App.DbHelper.GetAllParty();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            Xamarin.Forms.DependencyService.Get<IAuthHelper>().Authenticate();
        }
    }
}
