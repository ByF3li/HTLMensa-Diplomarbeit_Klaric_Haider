using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
using MensaHandyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class WarenkorbViewModel : IQueryAttributable
    {
        [ObservableProperty]
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();

        [ObservableProperty]
        private ObservableCollection<ShoppingCart> _shoppingcart = new ObservableCollection<ShoppingCart>();

        public int GetMenuId { get; private set; }
        private Menu selectedListItem;
        public Menu SelectedListItem
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
                        SendAlert(selectedListItem.MenuId);
                    }
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            GetMenuId = ((int)((int)query["MenuId"] as int?));
            OnPropertyChanged("MenuId");
            SetMenuId(GetMenuId);
        }

        public async void SetMenuId(int menuId)
        {

            if (menuId == null)
            {
                ShoppingCart noMenuFound = new ShoppingCart()
                {
                    ShoppingCartId = 0,
                };
                Shoppingcart.Add(noMenuFound);
            }
            else
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var client = new HttpClient(handler);
                Shoppingcart.Add(await client.GetFromJsonAsync<ShoppingCart>("https://oliverserver.ddns.net:7286/api/mensa/shoppingcart/getShoppingcartById/" + menuId));
                //Menus.Add(await client.GetFromJsonAsync<Menu>("https://oliverserver.ddns.net:7286/api/mensa/menu/getMenuById/" + menuId));
            }
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

                var client = new HttpClient(handler);

                Shoppingcart.RemoveAt(menuId);
                SelectedListItem = null;
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
            CmdPay = new AsyncRelayCommand(Pay);
        }

        public async Task Pay()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);

            String testMail = "testSchüler@tsn.at";
            List<int> menuIds = (List<int>) Shoppingcart.Select(sc => sc.MenuItems);
            
            DtoOrder order = new DtoOrder()
            {
                UserEmail = testMail,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                MenuIds = menuIds
            };

            await client.PostAsJsonAsync("https://oliverserver.ddns.net:7286/api/mensa/order/safeOrder", order);

            await Shell.Current.GoToAsync($"///OrderHistory");
            SelectedListItem = null;
        }


    }
}
