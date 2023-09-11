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
    public partial class ShowWeeklyMenuViewModel
    {

        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        public ShowWeeklyMenuViewModel()
        {
            Task t = ShowWeeklyMenuAsync();
        }

        private async Task ShowWeeklyMenuAsync()
        {
            HttpClient _client = new HttpClient();
            Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getThisWeeklyMenu");

        }
    }
}
