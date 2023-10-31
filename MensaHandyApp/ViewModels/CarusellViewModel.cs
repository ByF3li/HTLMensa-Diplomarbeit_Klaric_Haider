using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    partial class CarusellViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        public CarusellViewModel()
        {
            Task t = ShowMenu();
        }

        private async Task ShowMenu()
        {
            HttpClient _client = new HttpClient();
            //Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getThisWeeklyMenu");
            
            Menus.Add(new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Schnitzel mit Pommes",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            });

            Menus.Add(new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Tortellini mit Füllung",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 30)
            });

            Menus.Add(new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Steak",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            });

            Menus.Add(new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Hendl",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 31)
            });

            Menus.Add(new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 31)
            });

            Menus.Add(new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 10, 31)
            });

        }
    }
}
