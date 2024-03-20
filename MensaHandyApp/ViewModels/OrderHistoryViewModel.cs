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
        public string url = "https://oliverserver.ddns.net:7188/";
        //public string url = "https://localhost:7188/";

        [ObservableProperty]
        public ObservableCollection<MenuPerson> _orders = new ObservableCollection<MenuPerson>();

        private Person person;

        [ObservableProperty]
        public string _personemail; 
       
   
        public async Task ReloadData()
        {
            await ShowOrder();
        }

        private async Task ShowOrder()
        {
            Orders.Clear();

            person = await Person.LoadObject();
            Personemail = person.Email;

            var _client = Connect();
            try
            {
                person = await Person.LoadObject();

                List<MenuPerson> allOrders = new List<MenuPerson>();
                allOrders = await _client.GetFromJsonAsync<List<MenuPerson>>(url + "api/MenuPersonAPI/getAllOrderByUserEmail?mail=" + person.Email);

                allOrders.Reverse();

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

        public HttpClient Connect()
        {
            if (url == "https://localhost:7188/")
            {
                HttpClient _localhost_client = new HttpClient();
                return _localhost_client;
            }
            else if (url == "https://oliverserver.ddns.net:7188/")
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
