using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using MensaHandyApp.Models;
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
        private ObservableCollection<DayMenu> _dayMenus = new ObservableCollection<DayMenu>();

        public CarusellViewModel()
        {
            ShowMenu();
        }

        private async Task ShowMenu()
        {
            HttpClient _client = new HttpClient();
            //Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getThisWeeklyMenu");
            DayMenu dayMenu = new DayMenu()
            {
                Date=new DateOnly(2023, 10, 30)
            };
            Menu m1 = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Schnitzel mit Pommes",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            };
            dayMenu.Menus.Add(m1);

            Menu m2 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Tortellini mit Füllung",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 30)
            };
            dayMenu.Menus.Add(m2);

            Menu m3 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Steak",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            };

            dayMenu.Menus.Add(m3);
           
            DayMenu dayMenu1 = new DayMenu()
            {
                Date = new DateOnly(2023, 10, 31)
            };
            Menu m4 = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Hendl",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 31)
            };
            dayMenu1.Menus.Add(m4);

            Menu m5 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 31)
            };
            dayMenu1.Menus.Add(m5);

            Menu m6 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 10, 31)
            };
            dayMenu1.Menus.Add(m6);

            DayMenus.Add(dayMenu);
            DayMenus.Add(dayMenu1);
        }
    }
}
