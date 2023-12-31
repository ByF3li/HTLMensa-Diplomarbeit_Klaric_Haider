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
        private ObservableCollection<MenuPerson> _orders = new ObservableCollection<MenuPerson>();

        private Person person;

        public OrderHistoryViewModel()
        {
            Task t = ShowOrder();
        }
        private async Task ShowOrder()
        {
            person = await Person.LoadObject();

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var _client = new HttpClient(handler);
            try
            {
                List<MenuPerson> allOrders = new List<MenuPerson>();
                allOrders = await _client.GetFromJsonAsync<List<MenuPerson>>("https://oliverserver.ddns.net:7286/api/mensa/order/getAllOrderByUserEmail" + person.Email);
                // da hohl i mir alle Orders, brauch aber nur de von dieser Woche...
                // alte Orders (nit die neuerste), da sind die Menus wenn ma mit getOrderByUserEmail drüberfährt nit drinnen => aus zwischentabelle holen
                foreach (var order in allOrders)
                {
                    Orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    
    }
}
