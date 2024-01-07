using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class WarenkorbViewModel
    {
        public string url = "https://oliverserver.ddns.net/";
        //public string url = "https://localhost:7188/";

        private Person person;
        [ObservableProperty]
        List<MenuPerson> _shoppingcart = new List<MenuPerson>();

        [ObservableProperty]
        private decimal _priceOfMenu;

        private MenuPerson selectedListItem;
        public MenuPerson SelectedListItem
        {
            get
            {
                return selectedListItem;
            }
            set
            {
                if (selectedListItem != value)
                {
                    selectedListItem = value;
                    OnPropertyChanged("SelectedListItem");

                    if (selectedListItem != null)
                    {
                        SendAlert(selectedListItem.Menu.MenuId);
                    }
                }
            }
        }
        
        public async void GetShoppingCart()
        {
            var _client = Connect();

            List<MenuPerson> mps = await _client.GetFromJsonAsync<List<MenuPerson>>(url + "api/MenuPersonAPI/getAllOrderByUserEmail?mail=" + person.Email);

            person.MenuPersons.Clear();
            person.MenuPersons.AddRange(mps);
           // person.SaveObject();
            Shoppingcart = mps.Where(mp => mp.InShoppingcart).ToList();

            foreach(MenuPerson mp in Shoppingcart)
            {
                //testuser@gmx.at exampel for student
                if (person.Email == "testuser@gmx.at")
                {
                    PriceOfMenu = mp.Menu.Prices.PriceStudent;
                }
                //testuser2@gmx.at exampel for teacher
                else if (person.Email == "testuser2@gmx.at")
                {
                    PriceOfMenu = mp.Menu.Prices.PriceTeacher;
                }
                else
                {
                    PriceOfMenu = 9999.99m;
                }
            } 
        }
        
        private async void SendAlert(int menuId)
        {
            bool answer = await Shell.Current.DisplayAlert("Entfernen", "Soll das Menü vom Warenkorb entfernt werden", "Ja", "Nein");
            if (answer)
            {
                await Shell.Current.DisplayAlert("Ja", "Das Menü wird entfernt", "OK");

                var _client = Connect();

                var requestUri = url + $"api/MenuPersonAPI/deleteOrderByMenuId?userEmail={Uri.EscapeDataString(person.Email)}&menuId={menuId}";

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(requestUri),
                };

                using var response = await _client.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Fehler", "Das Menü wird entfernt", "OK");
                    throw new Exception("Order konnte nicht gelöscht werden");
                }
                else
                {
                    GetShoppingCart();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Nein", "Das Menü wird nicht entfernt", "OK");
                SelectedListItem = null;
            }
        }

        public IAsyncRelayCommand CmdPay { get; set; }

        public WarenkorbViewModel()
        {
            _ = Setup();
            CmdPay = new AsyncRelayCommand(Pay);
        }

        public async Task Setup()
        {
            person = await Person.LoadObject();
        }

        public async Task Pay()
        {
            if (Shoppingcart.Count() > 0)
            {
                var _client = Connect();

                var requestUri = url + $"api/MenuPersonAPI/updatePayedOrder?userEmail={Uri.EscapeDataString(person.Email)}";


                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(requestUri),
                };
                using var response = await _client.SendAsync(requestMessage);

                await Shell.Current.GoToAsync($"///OrderHistory");
                SelectedListItem = null;
            }
            else
            {
                await Shell.Current.DisplayAlert("Fehler", "Der Warenkorb ist leer", "OK");
            }
        }

        public void ReloadData()
        {
            GetShoppingCart();
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
