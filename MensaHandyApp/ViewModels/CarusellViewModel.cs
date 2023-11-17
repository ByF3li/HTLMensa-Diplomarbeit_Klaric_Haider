using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using MensaHandyApp.Models;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

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
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var client = new HttpClient(handler);
                
                // Fetch the weekly menus from the API
                var response = await client.GetFromJsonAsync<List<Menu>>("https://84.113.2.195:7286/api/mensa/menu/getThisWeeklyMenu");


                if (response != null && response.Count >= 15)
                {
                    // Create DayMenus for each day of the week
                    DateOnly resultDateOnly = ReturnThisWeek();
                    for (int i = 0; i < 5; i++)
                    {
                        DayMenu dayMenu = new DayMenu
                        {
                            Date = resultDateOnly.AddDays(i - 3),
                            Menus = new List<Menu>
                    {
                        response[i * 3],
                        response[i * 3 + 1],
                        response[i * 3 + 2]
                    }
                        };
                        DayMenus.Add(dayMenu);
                    }
                }
                else
                {
                    DayMenu testFailed = new DayMenu
                    {
                        Date = DateOnly.MaxValue,
                        Menus = new List<Menu>
                    {
                        new Menu
                        {
                            MenuId = 1,
                            WhichMenu = 0,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Price = 9999.99m,
                            Date = DateOnly.MaxValue
                        },
                        new Menu
                        {
                            MenuId = 2,
                            WhichMenu = 0,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Price = 9999.99m,
                            Date = DateOnly.MaxValue
                        },
                        new Menu
                        {
                            MenuId = 3,
                            WhichMenu = 0,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Price = 9999.99m,
                            Date = DateOnly.MaxValue
                        }
                    }
                    };
                    DayMenus.Add(testFailed);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle the specific exception related to the HTTP request
                Console.WriteLine("HttpRequestException: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur during the API request or data processing
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public DateOnly ReturnThisWeek()
        {

            DateTime dateTimeToday = DateTime.Now;

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = dfi.Calendar;
            int weekOfYear = calendar.GetWeekOfYear(dateTimeToday, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            int year = DateTime.Now.Year;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            var resultDateTime = firstThursday.AddDays(weekNum * 7);

            DateOnly resultDateOnly = DateOnly.FromDateTime(resultDateTime);

            return resultDateOnly;
        }
    }
}
