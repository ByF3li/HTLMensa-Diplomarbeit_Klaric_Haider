using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MensaHandyApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        /*
        private Person person;
        private string email;
        */
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }


        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(OnLogin);

            MessagingCenter.Subscribe<MainPageViewModel>(this, "LogoutSuccess", (sender) =>
            {
                LogoutSuccess();
            });
        }

        public void LogoutSuccess()
        {
            //email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        public async Task OnLogin()
        {
            //person = await Person.LoadObject();
            //email = person.Email;

            MessagingCenter.Send(this, "LoginSuccess");
            //SaveObject Person
            //Alle anderen SavePerson raus
            await Shell.Current.GoToAsync($"///MainPage");

            /*
            if (AuthenticateWithLDAP())
            {
                MessagingCenter.Send(this, "LoginSuccess");
                await Shell.Current.GoToAsync($"///MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Anmeldung fehlgeschlagen", "Benutzername oder Password sind Falsch", "OK");
            }
            */
        }


        /*
        private bool AuthenticateWithLDAP()
        {
            
            try
            {
                // Replace the LDAP server details with your own
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect("your-ldap-server", ldapport);
                    connection.Bind($"cn={Username},ou=users,dc=example,dc=com", Password);
                    return connection.Bound;
                }
            }
            catch (LdapException)
            {
                return false;
            }
            
        }
        */

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
