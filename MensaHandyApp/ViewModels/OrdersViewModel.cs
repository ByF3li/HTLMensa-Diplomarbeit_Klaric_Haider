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
        private ObservableCollection<Order> _orderses = new ObservableCollection<Order>();
        
        public OrdersViewModel()
        {
            Task t = ShowOrder();
        }
        private async Task ShowOrder()
        {
            Order o1 = new Order()
            {
                OrderId = 1,
                UserEmail = "fehaider@tsn.at",
                OrderDate = new DateOnly(2023, 11, 9)
            };
            Orderses.Add(o1);

            /*
            HttpClient _client = new HttpClient();
            Orders = await _client.GetFromJsonAsync<ObservableCollection<Order>>("https://localhost:7286/api/mensa/order/getAll");
            */
        }
    }
}
