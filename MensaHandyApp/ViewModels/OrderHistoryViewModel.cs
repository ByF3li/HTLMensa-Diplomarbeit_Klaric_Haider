using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class OrderHistoryViewModel
    {
    
        [ObservableProperty]
        public ObservableCollection<MenuPerson> _orders = new ObservableCollection<MenuPerson>();

        private Person person;

        [ObservableProperty]
        public string _personemail; 
       
   
        public async Task ReloadData()
        {
            // Perform actions to reload data
            await ShowOrder();
        }


        private async Task ShowOrder()
        {
            person = await Person.LoadObject();
            Personemail = person.Email;

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
                allOrders = await _client.GetFromJsonAsync<List<MenuPerson>>("https://oliverserver.ddns.net:7286/api/mensa/order/getAllOrderByUserEmail?mail=" + person.Email);

                allOrders.Reverse(); //The newest Order should be first

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
