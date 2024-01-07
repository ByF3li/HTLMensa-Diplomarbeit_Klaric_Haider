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
        public string url = "https://oliverserver.ddns.net/";
        //public string url = "https://localhost:7188/";

        private Person person  = new Person()
        {
            Email = "testuser@gmx.at",
            Password = "hallo123"
        };

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        [ObservableProperty]
        private ObservableCollection<DayMenu> _dayMenus = new ObservableCollection<DayMenu>();

        [ObservableProperty]
        private DayMenu _dayMenu;

        [ObservableProperty]
        private decimal _priceOfMenu;


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
                List<MenuPerson> mps = await _client.GetFromJsonAsync<List<MenuPerson>>(url + "api/MenuPersonAPI/getAllOrderByUserEmail?mail=" + person.Email);
                

                // um Bug zu lösen, wegen 400 response
                mps.ForEach(mp => {
                    mp.Person = person;
                    mp.Menu.MenuPersons.Clear();
                });

                Menu menu = selectedListItem;

                //testuser@gmx.at exampel for student
                if (person.Email == "testuser@gmx.at")
                {
                    PriceOfMenu = menu.Prices.PriceStudent;
                }
                //testuser2@gmx.at exampel for teacher
                else if (person.Email == "testuser2@gmx.at")
                {
                    PriceOfMenu = menu.Prices.PriceTeacher;
                }
                else
                {
                    PriceOfMenu = 9999.99m;
                }

                MenuPerson mp = new MenuPerson()
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    Payed = false,
                    Activated = false,
                    Person = person,
                    Menu = menu,
                    InShoppingcart = true,
                    MenuPersonId = 0
                };
                mp.Menu.MenuPersons.Clear();
                mps.Add(mp);
                //person.SaveObject();
                List<MenuPerson> shoppingcart = mps.Where(mp => mp.InShoppingcart).ToList();
                
                Console.WriteLine(await _client.PostAsJsonAsync(url + "api/MenuPersonAPI/saveOrder", shoppingcart, options));


                selectedListItem = new();
                await Shell.Current.GoToAsync($"///Warenkorb");

                //PerformNavigation(SelectedListItem.MenuId);
            }
            else
            {
                await Shell.Current.DisplayAlert("Nein", "Das Menü wird nicht hinzugefügt", "OK");
                SelectedListItem = null;
            }
        }

        /*
        private async void PerformNavigation(int? GetMenuId)
        {
            if (GetMenuId != null)
            {
                var navigationParameter = new Dictionary<string, object> { { "MenuId", GetMenuId } };
                SelectedListItem = null;
                await Shell.Current.GoToAsync($"///Warenkorb", navigationParameter);
            }
            SelectedListItem = null;
        }
        */

        public WeeklyMenusViewModel()
        {
            ShowMenu();
        }

        private async Task ShowMenu()
        {
            try
            {
                var _client = Connect();

                // Fetch the weekly menus from the API
                var response = await _client.GetFromJsonAsync<List<Menu>>(url + "api/MenuAPI/getThisWeeklyMenu");


                if (response != null ) //&& response.Count >= 15)
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
                // Handle the specific exception related to the HTTP request
                Console.WriteLine("HttpRequestException: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur during the API request or data processing
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            await GetCarusellPositionAsync();
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
        

        //Pos wär eigentlich richtig aber wird nicht in view übernommen 
        public async Task GetCarusellPositionAsync()
        {
            DateOnly dateThisWeek = ReturnThisWeek();

            DateOnly monday = dateThisWeek.AddDays(-3); //Monday
            DateOnly tuesday = dateThisWeek.AddDays(-2); //Tuesday
            DateOnly wednesday = dateThisWeek.AddDays(-1); //Wednesday
            DateOnly thursday = dateThisWeek;             //Thursday
            DateOnly friday = dateThisWeek.AddDays(+1); //Friday
            DateOnly saturaday = dateThisWeek.AddDays(+2); //Saturday
            DateOnly sunday = dateThisWeek.AddDays(+3); //Sunday

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (today == monday || today == saturaday || today == sunday)
            {
                DayMenu = DayMenus[0];
            }
            else if (today == tuesday)
            {
                DayMenu = DayMenus[1];
            }
            else if (today == wednesday)
            {
                DayMenu = DayMenus[2];
            }
            else if (today == thursday)
            {
                DayMenu = DayMenus[3];
            }
            else if (today == friday)
            {
                DayMenu = DayMenus[4];
            }
            else
            {
                DayMenu = DayMenus[0];
                await Shell.Current.DisplayAlert("Fehler", "GetCarusellPosition geht nicht", "OK");
            }
            //await Shell.Current.DisplayAlert("pos", pos, "OK");
        }

        public HttpClient Connect()
        {
            if(url == "https://localhost:7188/")
            {
                HttpClient _localhost_client = new HttpClient();
                return _localhost_client;
            }
            else if (url == "https://oliverserver.ddns.net/")
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
