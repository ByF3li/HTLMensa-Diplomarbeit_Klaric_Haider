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
    public partial class WarenkorbViewModel : IQueryAttributable
    {
        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        public int GetMenuId { get; private set; }

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
                Menu noMenuFound = new Menu()
                {
                    WhichMenu = 1,
                    Starter = "Gibt keine",
                    MainCourse = "Du hund kannsch nit programmieren",
                    Price = 99999.99m,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                Menus.Add(noMenuFound);
            }

            Menu menuTest = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Schnitzel mit Pommes",
                Price = 6.99m,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };
            Menus.Add(menuTest);

            /*
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler); 
            Menus.Add(await client.GetFromJsonAsync<Menu>("https://213.47.166.108:7286/api/mensa/menu/getMenuById/" + menuId));
            */

        }
    }
}
