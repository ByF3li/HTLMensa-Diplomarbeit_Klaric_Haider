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

        private Menu selectedListItem;
        public Menu SelectedListItem
        {
            get
            {
                return selectedListItem;
            }
            set
            {
                if (selectedListItem != value)
                {
                    selectedListItem = value;
                    OnPropertyChanged("SelectedListItem");

                    if (selectedListItem != null)
                    {
                        SendAlert();
                    }
                }
            }
        }

        private async void SendAlert()
        {
            bool answer = await Shell.Current.DisplayAlert("Hinzufügen", "Soll das Menü dem Warenkorb hinzugefügt werden", "Ja", "Nein");
            if (answer)
            {
                await Shell.Current.DisplayAlert("Ja", "Das Menü wird hinzugefügt", "OK");
                PerformNavigation(SelectedListItem.MenuId);
            }
            else
            {
                await Shell.Current.DisplayAlert("Nein", "Das Menü wird nicht hinzugefügt", "OK");
                SelectedListItem = null;
            }
        }

        private async void PerformNavigation(int? GetMenuId)
        {
            if (GetMenuId != null)
            {
                var navigationParameter = new Dictionary<string, object> { { "MenuId", GetMenuId } };
                await Shell.Current.GoToAsync($"///Warenkorb", navigationParameter);
            }

            SelectedListItem = null;
        }

        public CarusellViewModel()
        {
            ShowMenu();
        }

        private async Task ShowMenu()
        {
            HttpClient _client = new HttpClient();
            //Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getThisWeeklyMenu");
            
            //Hardcoded till DB works
            DayMenu monday = new DayMenu()
            {
                Date=new DateOnly(2023, 10, 30)
            };
            Menu m1 = new Menu()
            {
                MenuId = 1,
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Schnitzel mit Pommes",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            };
            monday.Menus.Add(m1);

            Menu m2 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Tortellini mit Füllung",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 30)
            };
            monday.Menus.Add(m2);

            Menu m3 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Steak",
                Price = 6.99m,
                Date = new DateOnly(2023, 10, 30)
            };
            monday.Menus.Add(m3);
           
            DayMenu tuesday = new DayMenu()
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
            tuesday.Menus.Add(m4);

            Menu m5 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 10, 31)
            };
            tuesday.Menus.Add(m5);

            Menu m6 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 10, 31)
            };
            tuesday.Menus.Add(m6);

            DayMenu wednesday = new DayMenu()
            {
                Date = new DateOnly(2023, 11, 1)
            };
            Menu m7 = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Hendl",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 1)
            };
            wednesday.Menus.Add(m4);

            Menu m8 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 1)
            };
            wednesday.Menus.Add(m5);

            Menu m9 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 11, 1)
            };
            wednesday.Menus.Add(m6);

            DayMenu thursday = new DayMenu()
            {
                Date = new DateOnly(2023, 11, 2)
            };
            Menu m10 = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Hendl",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 2)
            };
            thursday.Menus.Add(m4);

            Menu m11 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 2)
            };
            thursday.Menus.Add(m5);

            Menu m12 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 11, 2)
            };
            thursday.Menus.Add(m6);

            DayMenu friday = new DayMenu()
            {
                Date = new DateOnly(2023, 11, 3)
            };
            Menu m13 = new Menu()
            {
                WhichMenu = 1,
                Starter = "Eisbergsalat",
                MainCourse = "Hendl",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 3)
            };
            friday.Menus.Add(m4);

            Menu m14 = new Menu()
            {
                WhichMenu = 2,
                Starter = "Eisbergsalat",
                MainCourse = "Wassermelone",
                Price = 7.99m,
                Date = new DateOnly(2023, 11, 3)
            };
            friday.Menus.Add(m5);

            Menu m15 = new Menu()
            {
                WhichMenu = 3,
                Starter = "Eisbergsalat",
                MainCourse = "Bernerwürstel mit Pommes",
                Price = 8.99m,
                Date = new DateOnly(2023, 11, 3)
            };
            friday.Menus.Add(m6);

            DayMenus.Add(monday);
            DayMenus.Add(tuesday);
            DayMenus.Add(wednesday);
            DayMenus.Add(thursday);
            DayMenus.Add(friday);
        }
    }
}
