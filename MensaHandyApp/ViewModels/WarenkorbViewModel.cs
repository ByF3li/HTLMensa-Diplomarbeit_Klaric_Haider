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

        private Person person;
        [ObservableProperty]
        List<MenuPerson> _shoppingcart = new List<MenuPerson>();


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
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            var client = new HttpClient(handler);

            List<MenuPerson> mps = await client.GetFromJsonAsync<List<MenuPerson>>("https://oliverserver.ddns.net:7286/api/mensa/order/getAllOrderByUserEmail?mail=" + person.Email);
            person.MenuPersons.Clear();
            person.MenuPersons.AddRange(mps);
           // person.SaveObject();
            Shoppingcart = mps.Where(mp => mp.InShoppingcart).ToList();
        }
        
        private async void SendAlert(int menuId)
        {
            bool answer = await Shell.Current.DisplayAlert("Entfernen", "Soll das Menü vom Warenkorb entfernt werden", "Ja", "Nein");
            if (answer)
            {
                await Shell.Current.DisplayAlert("Ja", "Das Menü wird entfernt", "OK");

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var _client = new HttpClient(handler);

                var requestUri = $"https://oliverserver.ddns.net:7286/api/mensa/order/deleteOrderByMenuId?userEmail={Uri.EscapeDataString(person.Email)}&menuId={menuId}";

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
            Setup();
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
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var _client = new HttpClient(handler);

                var requestUri = $"https://oliverserver.ddns.net:7286/api/mensa/order/updatePayedOrder?userEmail={Uri.EscapeDataString(person.Email)}";

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
            // Perform actions to reload data
            GetShoppingCart();
        }
    }
}
