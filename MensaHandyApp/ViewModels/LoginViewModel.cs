using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UIKit;

namespace MensaHandyApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string url = "https://oliverserver.ddns.net/";
        //public string url = "https://localhost:7188/";

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
            /*
            if(Person.LoadObject() != null)
            {
                MessagingCenter.Send(this, "LoginSuccess");
                await Shell.Current.GoToAsync($"///MainPage");
            }
            else
            {
            */
                HttpResponseMessage check = await AuthentAsync();

                if (check != null)
                {
                    Console.WriteLine(check.Content);

                    Person person = new Person()
                    {
                        Email = Email,
                        FirstName = "",
                        LastName = "",
                        IsTeacher = true,
                    };
                    person.SaveObject();

                    MessagingCenter.Send(this, "LoginSuccess");
                    await Shell.Current.GoToAsync($"///MainPage");
                }
                else
                {
                    Email = "";
                    Password = "";
                    await Shell.Current.DisplayAlert("Anmeldung fehlgeschlagen", "Email oder Passwort Falsch", "OK");
                }
            //}
            
        }

        public async Task<HttpResponseMessage> AuthentAsync()
        {

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
                    return response;
                }
                return null;

            }
            return null;
        }

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

        public static string Base64Decode(string base64)
        {
            var base64Bytes = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }
    }
}
