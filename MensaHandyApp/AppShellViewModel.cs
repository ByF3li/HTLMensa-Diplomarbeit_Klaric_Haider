using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using MensaHandyApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaHandyApp
{
    public partial class AppShellViewModel : ObservableObject, INotifyPropertyChanged
    {
        //private Person person;

        private bool _isHomepageVisible = true;
        private bool _isWeeklyMenusVisible = false;
        private bool _isWarenkorbVisible = false;
        private bool _isOrderHistoryVisible = false;
        private bool _isLoginVisible = true;

        public bool IsHomepageVisible
        {
            get { return _isHomepageVisible; }
            set
            {
                if (_isHomepageVisible != value)
                {
                    _isHomepageVisible = value;
                    OnPropertyChanged(nameof(IsHomepageVisible));
                }
            }
        }

        public bool IsWeeklyMenusVisible
        {
            get { return _isWeeklyMenusVisible; }
            set
            {
                if (_isWeeklyMenusVisible != value)
                {
                    _isWeeklyMenusVisible = value;
                    OnPropertyChanged(nameof(IsWeeklyMenusVisible));
                }
            }
        }

        public bool IsWarenkorbVisible
        {
            get { return _isWarenkorbVisible; }
            set
            {
                if (_isWarenkorbVisible != value)
                {
                    _isWarenkorbVisible = value;
                    OnPropertyChanged(nameof(IsWarenkorbVisible));
                }
            }
        }

        public bool IsOrderHistoryVisible
        {
            get { return _isOrderHistoryVisible; }
            set
            {
                if (_isOrderHistoryVisible != value)
                {
                    _isOrderHistoryVisible = value;
                    OnPropertyChanged(nameof(IsOrderHistoryVisible));
                }
            }
        }

        public bool IsLoginVisible
        {
            get { return _isLoginVisible; }
            set
            {
                if (_isLoginVisible != value)
                {
                    _isLoginVisible = value;
                    OnPropertyChanged(nameof(IsLoginVisible));
                }
            }
        }

        public IAsyncRelayCommand ShowUserCommand { get; set; }

        public AppShellViewModel()
        {
            //_ = Setup();

            ShowUserCommand = new AsyncRelayCommand(ShowUser);

            /*
            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccess", (sender) =>
            {
                //person.Email = email;
            });

            MessagingCenter.Subscribe<MainPageViewModel>(this, "LogoutSuccess", (sender) =>
            {
                //person.Email = "";         
            });
            */
        }

        /*
        public async Task Setup()
        {
            person = await Person.LoadObject();
        }
        */

        public async Task ShowUser()
        {
            //Make dropdown menu
            await Shell.Current.DisplayAlert("UserIcon angeklickt", "Funktionen kommen noch", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
