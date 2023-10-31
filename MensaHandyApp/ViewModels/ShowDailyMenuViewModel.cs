using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MensaAppKlassenBibliothek;
using System.Collections.Specialized;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class ShowDailyMenuViewModel : IQueryAttributable
    {
        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        String Text { get; set; } = "Hallo";

        public ShowDailyMenuViewModel()
        {
           //Task t = ShowDailyMenuAsync();
        }

        public DateOnly GetDate {  get; private set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            GetDate = ((DateOnly)((DateOnly)query["GetDate"] as DateOnly?));
            OnPropertyChanged("GetDate");
            SetDate(GetDate);
        }

        /*
        private async Task ShowDailyMenuAsync()
        {
            HttpClient _client = new HttpClient();
            Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getDailyMenu");
        }
        */

        public async void SetDate(DateOnly date)
        {
            if (date == null)
            {
                return;
            }
            HttpClient _client = new HttpClient();
            Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getMenuByDate/" + date.Year + "-" + date.Month + "-" + date.Day);
        }
    }
}
