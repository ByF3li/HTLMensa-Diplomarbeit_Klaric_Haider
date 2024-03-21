using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MensaHandyApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string url = "input_your_server_ip";
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

            Person.DeleteObject();
        }

        public async Task OnLogin()
        {
            Person check = await AuthentAsync();
            
            HttpClient _client = Connect();

            if (check != null)
            {
                check.Email = Email;
                var response = await _client.PostAsJsonAsync<Person>($"{url}api/PersonAPI/addPerson", check);
                await check.SaveObject();
                Email = "";

                MessagingCenter.Send(this, "LoginSuccess");
                await Shell.Current.GoToAsync($"///MainPage");
            }
            else
            {
                Email = "";
                Password = "";
                await Shell.Current.DisplayAlert("Fehler", "Username und Password müssen befüllt werden", "OK");
            }
        }

        public async Task<Person> AuthentAsync()
        {
            if ((Email == "Admin@Admin") && (Password == "Admin"))
            {
                Person p = new Person()
                {
                    Email = Email,
                    FirstName = "Admin",
                    LastName = "Admin",
                    IsTeacher = true
                };
                return p;
            }
            else if ((Email != "") && (Password != ""))
            {
                var _client = Connect();

                var requestUri = $"{url}api/LdapAPI/getLDAP?username={Uri.EscapeDataString(Email)}" + 
                                        $"&password={Uri.EscapeDataString(Password)}";
                                
                
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUri),
                };
                var response = await _client.GetFromJsonAsync<Person>(requestUri);
                Password = "";

                return response;
            }
            else
            {
                Email = "";
                Password = "";
                await Shell.Current.DisplayAlert("Fehler", "Username und Password müssen befüllt werden", "OK");
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
            else if (url == "your_server_ip")
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var _server_client = new HttpClient(handler);

                return _server_client;
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
