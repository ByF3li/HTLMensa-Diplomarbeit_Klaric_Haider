using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class OrderHistoryViewModel
    {
    
        [ObservableProperty]
        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        
        public OrderHistoryViewModel()
        {
            Task t = ShowOrder();
        }
        private async Task ShowOrder()
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

            try
            {
                List<Order> o = new List<Order>();
                o = await client.GetFromJsonAsync<List<Order>>("https://oliverserver.ddns.net:7286/api/mensa/order/getOrderByUserEmail?mail=" + testMail);
                // da hohl i mir alle Orders, brauch aber nur de von dieser Woche...
                // alte Orders (nit die neuerste), da sind die Menus wenn ma mit getOrderByUserEmail drüberfährt nit drinnen => aus zwischentabelle holen
                foreach (var order in o)
                {
                    Orders.Add(order);
                }
            }
            catch (Exception ex)
            {

            }
            
        }
    
    }
}
