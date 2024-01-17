using CommunityToolkit.Mvvm.Input;
using Java.Net;
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

            bool check = await AuthentAsync();

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

        public async Task<bool> AuthentAsync()
        {
            string url = "https://oliverserver.ddns.net/";

            if ((Email != "") && (Password != ""))
            {
                var _client = Connect();

                var requestUri = $"{url}api/LdapAPI/getLDAP?username={Uri.EscapeDataString(Email)}" + 
                                        $"&password={Uri.EscapeDataString(Password)}";
                                
                
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                };
                using var response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;

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
        public HttpClient Connect()
        {
            if (url == "https://localhost:7188/")
            {
                HttpClient _localhost_client = new HttpClient();
                return _localhost_client;
            }
            else if (url == "https://oliverserver.ddns.net/")
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var _oliverserver_client = new HttpClient(handler);

                return _oliverserver_client;
            }
            else
            {
                throw new Exception("Konnte nicht verbunden werden");
            }

        }
    }
}
