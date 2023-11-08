﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MensaAppKlassenBibliothek;
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
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        [ObservableProperty]
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();

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
                        SendAlert();
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
            List<int> takenMenuIds = Menus.Select(menu => menu.MenuId).ToList();

            if (menuId == null)
            {
                Menu noMenuFound = new Menu()
                {
                    MenuId = menuId,
                    WhichMenu = 1,
                    Starter = "Gibt keine",
                    MainCourse = "Du hund kannsch nit programmieren",
                    Price = 99999.99m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                Menus.Add(noMenuFound);
            }
            else if(takenMenuIds.Contains(menuId))
            {
                return;
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
                Menus.Add(await client.GetFromJsonAsync<Menu>("https://213.47.166.108:7286/api/mensa/menu/getMenuById/" + menuId));




                //denk nach Pati bitte 
                //warenkorb hinzufügen (tabelle)
                /*
                String testMail = "testSchüler@tsn.at";

                Order order = new Order()
                {
                    OrderId = 1,
                    UserEmail = "testSchüler@tsn.at",
                    OrderDate = new DateOnly(2023, 11, 9),
                    Menus = Menus.ToList()
                };
                Orders.Add(order);

                await client.PostAsJsonAsync<Order>("https://213.47.166.108:7286/api/mensa/order/safeOrder", order);

                Orders.Add(await client.GetFromJsonAsync<Order>("https://213.47.166.108:7286/api/mensa/order/getOrderByUserEmail/" + testMail));
                */

                //todo warenkorb löschen nach kauf (von warenkorb -> kaufen -> order)
                //2 tabellen gekauft(order) und warenkorb
                //email aus order löschen (freitag abend)

                //Warenkorb button der zu bezahlung führt rechts in der ecke


            }
        }

        private async void SendAlert()
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
                //Menus.Remove(await client.GetFromJsonAsync<Menu>("https://213.47.166.108:7286/api/mensa/menu/getMenuByDate/" + SelectedListItem.MenuId));
                Menu deleteErfolgreich = new Menu()
                {
                    MenuId = 420,
                    WhichMenu = 1,
                    Starter = "erfolgreich",
                    MainCourse = "Gelöscht",
                    Price = 6.99m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                Menus.Add(deleteErfolgreich);

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

            Order order = new Order()
            {
                UserEmail = testMail,
                OrderDate = new DateOnly(2023, 11, 9),
                Menus = Menus.ToList()
            };
            Orders.Add(order);

            await client.PostAsJsonAsync<Order>("https://213.47.166.108:7286/api/mensa/order/safeOrder", order);

            await Shell.Current.GoToAsync($"///Orders");
        }
    }
}
