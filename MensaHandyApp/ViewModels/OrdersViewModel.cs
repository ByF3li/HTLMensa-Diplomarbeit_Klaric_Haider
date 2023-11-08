using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class OrdersViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        
        public OrdersViewModel()
        {
            Task t = ShowOrder();
        }
        private async Task ShowOrder()
        {
            String testMail = "testSchüler@tsn.at";

            /*
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);

            //bitte nit hardcoden danke
            

            //mach a liste drauß weil mehrere Orders pro person sein können
            //Onclick auf die Order -> neue View mit alle Menüs zu dem Order 
            Orders.Add(await client.GetFromJsonAsync<Order>("https://213.47.166.108:7286/api/mensa/order/getOrderByUserEmail/" + testMail));
            */

            Order order = new Order()
            {
                OrderId = 1,
                UserEmail = testMail,
                OrderDate = new DateOnly(2023, 11, 9),
            };
            Orders.Add(order);
        }
    }
}
