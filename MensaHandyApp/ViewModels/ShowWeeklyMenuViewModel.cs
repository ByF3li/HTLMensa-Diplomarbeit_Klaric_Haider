using CommunityToolkit.Mvvm.ComponentModel;
using MensaAppKlassenBibliothek;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MensaHandyApp.ViewModels
{
    [ObservableObject]
    public partial class ShowWeeklyMenuViewModel
    {

        [ObservableProperty]
        private ObservableCollection<Menu> _menus = new ObservableCollection<Menu>();

        private Menu selectedListItem;
        public Menu SelectedListItem { 
            get
            {
                return selectedListItem;
            }
            set
            {
                if(selectedListItem != value) 
                {
                    selectedListItem = value;
                    OnPropertyChanged("SelectedListItem");

                    if(selectedListItem != null)
                    {
                        PerformNavigation(SelectedListItem.Date);
                    }
                }
            }
        }


        private async void PerformNavigation(DateOnly? GetDate)
        {
            DateOnly resultDateOnly = ReturnThisWeek();

            if (GetDate != null)
            {
                var navigationParameter = new Dictionary<string, object> { { "GetDate", GetDate } };
                await Shell.Current.GoToAsync($"///ShowDailyMenuView", navigationParameter);
            }
       
            SelectedListItem = null;
        }
       

        public ShowWeeklyMenuViewModel()
        {
            Task t = ShowWeeklyMenuAsync();
        }

        private async Task ShowWeeklyMenuAsync()
        {
            HttpClient _client = new HttpClient();
            Menus = await _client.GetFromJsonAsync<ObservableCollection<Menu>>("https://localhost:7286/api/mensa/menu/getThisWeeklyMenu");

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
