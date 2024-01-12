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
    // Has LoginSucces.publish
    // Has LogoutSucces.subscribe

    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
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
            Email = "";
            Password = "";
        }

        public async Task OnLogin()
        {
            Console.WriteLine($"Email: {Email}, Password: {Password}");

            bool check = Authent();

            if (check)
            {
                MessagingCenter.Send(this, "LoginSuccess");
                await Shell.Current.GoToAsync($"///MainPage");
            }
            else
            {
                Email = "";
                Password = "";
                await Shell.Current.DisplayAlert("Anmeldung fehlgeschlagen", "Email oder Passwort Falsch", "OK");
            }
           
            
            
            //SaveObject Person
            //Alle anderen SavePerson raus

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

        public bool Authent()
        {

            if ((Email != "") && (Email.Contains('@')) && (Password != ""))
            {
                return true;
            }
            return false;
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
