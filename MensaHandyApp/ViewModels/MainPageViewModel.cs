using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MensaHandyApp.ViewModels
{
    public class MainPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        private bool _showLoginButton = true;
        private bool _showLogoutButton = false;

        public bool ShowLoginButton
        {
            get { return _showLoginButton; }
            set { SetProperty(ref _showLoginButton, value); }
        }

        public bool ShowLogoutButton
        {
            get { return _showLogoutButton; }
            set { SetProperty(ref _showLogoutButton, value); }
        }

        public IAsyncRelayCommand LoginCommand { get; set; }
        public IAsyncRelayCommand LogoutCommand { get; set; }

        public MainPageViewModel()
        {
            LoginCommand = new AsyncRelayCommand(OpenLoginPage);
            LogoutCommand = new AsyncRelayCommand(LogoutPerson);

            MessagingCenter.Subscribe<LoginViewModel>(this, "LoginSuccess", (sender) =>
            {
                LoginSuccess();
            });
        }

        public void LoginSuccess()
        {
            ShowLogoutButton = true;
            ShowLoginButton = false;
        }

        public async Task OpenLoginPage()
        {
            await Shell.Current.GoToAsync($"///Login"); 
        }

        public async Task LogoutPerson()
        {
            ShowLogoutButton = false;
            ShowLoginButton = true;

            MessagingCenter.Send(this, "LogoutSuccess");
        }
    }
}
