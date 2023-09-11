using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MensaAppKlassenBibliothek;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class ShowDailyMenuViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        public ShowDailyMenuViewModel()
        {
            Task t = ShowDailyMenuAsync();
        }

        private async Task ShowDailyMenuAsync()
        {
            HttpClient _client = new HttpClient();
            Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getDailyMenu");
        }
    }
}
