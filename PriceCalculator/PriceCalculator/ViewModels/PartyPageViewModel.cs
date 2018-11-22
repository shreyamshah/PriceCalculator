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
	public class PartyPageViewModel : ViewModelBase
	{
        public DelegateCommand AddPartyCommand { get; set; }
        public PartyPageViewModel(INavigationService navigationService,IPageDialogService dialogService) : base(navigationService,dialogService)
        {
            Xamarin.Forms.MessagingCenter.Subscribe<Party>(this, "added", OnPartyAdded);
            AddPartyCommand = new DelegateCommand(AddParty);
            GetAllParty();
        }

        public void OnPartyAdded(Party obj)
        {
            if (obj != null)
            {
                GetAllParty();
            }
        }

        public void AddParty()
        {
            NavigationService.NavigateAsync("PartyAddPage");
        }

        private ObservableCollection<Party> partyList;
        public ObservableCollection<Party> PartyList
        {
            get { return partyList; }
            set { SetProperty(ref partyList, value); }
        }

        public void GetAllParty()
        {
            PartyList = new ObservableCollection<Party>(App.DbHelper.GetAllParty());
        }
    }
}
