using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using MensaHandyApp.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    partial class WeeklyMenusViewModel
    {
        public string url = "https://oliverserver.ddns.net:7188/";
        //public string url = "https://localhost:7188/";

        private Person person;

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        [ObservableProperty]
        private ObservableCollection<DayMenu> _dayMenus = new ObservableCollection<DayMenu>();

        [ObservableProperty]
        private DayMenu _dayMenu;

        private bool _teacherPrice = false;
        private bool _studentPrice = false;

        public bool ShowTeacherPrice
        {
            get { return _teacherPrice; }
            set { SetProperty(ref _teacherPrice, value); }
        }

        public bool ShowStudentPrice
        {
            get { return _studentPrice; }
            set { SetProperty(ref _studentPrice, value); }
        }


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

                var _client = Connect();

                person = await Person.LoadObject();

                List<MenuPerson> mps = await _client.GetFromJsonAsync<List<MenuPerson>>(url + "api/MenuPersonAPI/getAllOrderByUserEmail?mail=" + person.Email);


                // solving a 400 response
                mps.ForEach(mp =>
                {
                    mp.Person = person;
                    mp.Menu.MenuPersons.Clear();
                });

                Menu menu = selectedListItem;

                MenuPerson mp = new MenuPerson()
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    PaymentStatus = "Not Payed",
                    Activated = false,
                    Person = person,
                    Menu = menu,
                    InShoppingcart = true,
                    MenuPersonId = 0,
                    PaypalOrderId = ""
                };
                mp.Menu.MenuPersons.Clear();
                mps.Add(mp);
                List<MenuPerson> shoppingcart = mps.Where(mp => mp.InShoppingcart).ToList();

                await _client.PostAsJsonAsync(url + "api/MenuPersonAPI/saveOrder", shoppingcart, options);

                selectedListItem = new();
                await Shell.Current.GoToAsync($"///Warenkorb");
            }
            else
            {
                await Shell.Current.DisplayAlert("Nein", "Das Menü wird nicht hinzugefügt", "OK");
                SelectedListItem = null;
            }
        }

        public WeeklyMenusViewModel()
        {
            _ = Setup();
            ShowMenu();
        }

        public async Task Setup()
        {
            person = await Person.LoadObject();
        }

        private async Task ShowMenu()
        {
            try
            {
                var _client = Connect();

                var response = await _client.GetFromJsonAsync<List<Menu>>(url + "api/MenuAPI/getThisWeeklyMenu");


                if (person.IsTeacher)
                {
                    ShowTeacherPrice = true;
                    ShowStudentPrice = false;
                }
                else if (!person.IsTeacher)
                {
                    ShowTeacherPrice = false;
                    ShowStudentPrice = true;
                }


                if (response != null)
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
                    //For testing purposes
                    DayMenu testFailed = new DayMenu
                    {
                        Date = DateOnly.MaxValue,
                        Menus = new List<Menu>
                    {
                        new Menu
                        {
                            MenuId = 1,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Date = DateOnly.MaxValue
                        },
                        new Menu
                        {
                            MenuId = 2,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Date = DateOnly.MaxValue
                        },
                        new Menu
                        {
                            MenuId = 3,
                            Starter = "Fehler",
                            MainCourse = "Fehler",
                            Date = DateOnly.MaxValue
                        }
                    }
                    };
                    DayMenus.Add(testFailed);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HttpRequestException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        // Getting the thursday of the current week 
        // The ReturnThisWeek()-Method is made by stackoverflow and was later rewritten and adapted
        // 1. The MDSN Library Approach:
        //      Author: Amberlamps
        //      url: https://stackoverflow.com/q/11154673   
        // 2. And the Answer of Leidegren, John
        //      Author: Leidegren, John
        //      url: https://stackoverflow.com/a/5378150
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
