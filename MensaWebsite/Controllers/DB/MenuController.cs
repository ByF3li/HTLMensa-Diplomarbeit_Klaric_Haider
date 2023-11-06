using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Connections.Features;

namespace MensaWebAPI.Controllers
{

    [Route("api/mensa")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public MenuController(MenuContext context)
        {
            this._context = context;
        }


        [HttpGet]
        [Route("menu/getAll")]
        public async Task<IActionResult> AsyncGetAllMenues()
        {
            return new JsonResult(await this._context.Menues.Include("Orders").ToListAsync(), options);
        }

        [HttpPost]
        [Route("menu/safeMenu")]
        public async Task<IActionResult> AsyncSafeMenu(int whichMenu, string starter, string mainCourse, decimal price, DateOnly date)
        {
            Menu menu = new Menu()
            {
                WhichMenu = whichMenu,
                Starter = starter,
                MainCourse = mainCourse,
                Price = price,
                Date = date
            };
            this._context.Menues.Add(menu);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1, options);
        }

        [HttpPatch]
        [Route("menu/editMenu")]
        public async Task<IActionResult> AsyncEditMenu(int id, int whichMenu, string starter, string mainCourse, decimal price, DateOnly date)
        {
            var menu = await this._context.Menues.FindAsync(id);
            if (menu != null)
            {
                menu.WhichMenu = whichMenu;
                menu.Starter = starter;
                menu.MainCourse = mainCourse;
                menu.Price = price;
                menu.Date = date;
            }
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }

        [HttpGet]
        [Route("menu/getMenuByDate/{menuDate}")]
        public async Task<IActionResult> AsyncGetMenuByDate(DateOnly menuDate)
        {
            
            var toSortDate = await this._context.Menues.Include("Orders").Where(m => m.Date.Equals(menuDate)).ToListAsync();
            toSortDate.Sort();
            return new JsonResult(toSortDate, options);
        }

        [HttpGet]
        [Route("menu/getMenuById/{menuId}")]
        public async Task<IActionResult> AsyncGetMenuById(int menuId)
        {
            return new JsonResult(await this._context.Menues.FindAsync(menuId));
        }

        [HttpDelete]
        [Route("menu/deleteMenuById/{menuId}")]
        public async Task<IActionResult> AsyncDeleteMenuById(int menuId)
        {
            var articleToDelete = await this._context.Menues.FindAsync(menuId);

            if (articleToDelete == null)
            {
                return NotFound($"Article with Id = {menuId} not found");
            }

            this._context.Menues.Remove(articleToDelete);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);

        }

        [HttpGet]
        [Route("menu/getDailyMenu")]
        public async Task<IActionResult> AsyncGetDailyMenu()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            return new JsonResult(await this._context.Menues.Where(m => m.Date.Equals(today)).ToListAsync());
        }

        [HttpGet]
        [Route("menu/LetsGetItStartedAhLetsGetItStartedInHere")]
        public async Task<IActionResult> LetsGetItStartedAhLetsGetItStartedInHere()
        {
            DateTime dateTimeToday = DateTime.Now;

            //Gibt die Wochennummer aus 
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            int weekNr = cal.GetWeekOfYear(dateTimeToday, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            return new JsonResult(weekNr);

            //Gibt den heuten Tag aus
            DateOnly dateToday = DateOnly.FromDateTime(dateTimeToday);
            return new JsonResult(await this._context.Menues.Where(m => m.Date.Equals(dateToday)).ToListAsync());

            //todo
            //aus der wochennummer alle tage der woche rausbekommen z.B: woche 32 -> 07.08.2023 (Montag) - 13.08.2023 (Sonntag)
            //die Menüs von jeden Tag ausgeben 
        }

        //todo: year und weekofyear besetzen ohne in swagger eingeben zu müssen; dann sollt der scheiß funktionieren
        [HttpGet]
        [Route("menu/getWeeklyMenu")]
        public async Task<IActionResult> AsyncGetAnyWeeklyMenu(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var resultDateTime = firstThursday.AddDays(weekNum * 7);

            DateOnly resultDateOnly = DateOnly.FromDateTime(resultDateTime);
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            List<DateOnly> days = new List<DateOnly>()
            {
                resultDateOnly.AddDays(-3), //Monday
                resultDateOnly.AddDays(-2), //Tuesday
                resultDateOnly.AddDays(-1), //Wednesday
                resultDateOnly,             //Thursday
                resultDateOnly.AddDays(+1), //Friday
                resultDateOnly.AddDays(+2), //Saturday
                resultDateOnly.AddDays(+3)  //Sunday

            };

            //return new JsonResult(days);
            //return new JsonResult(await this._context.Menues.Where(days.Contains()).toListAsync());
            return new JsonResult(await this._context.Menues.Where(m => days.Contains(m.Date)).ToListAsync());
        }   

        [HttpGet]
        [Route("menu/getThisWeeklyMenu")]
        public async Task<IActionResult> AsyncGetThisWeeklyMenu()
        {
            DateOnly resultDateOnly = ReturnThisWeek();

            List<DateOnly> days = new List<DateOnly>()
            {
                resultDateOnly.AddDays(-3), //Monday
                resultDateOnly.AddDays(-2), //Tuesday
                resultDateOnly.AddDays(-1), //Wednesday
                resultDateOnly,             //Thursday
                resultDateOnly.AddDays(+1), //Friday
                //resultDateOnly.AddDays(+2), //Saturday
                //resultDateOnly.AddDays(+3)  //Sunday
            };

            List<Menu> menuList = new List<Menu>();
            menuList = await this._context.Menues.Where(m => days.Contains(m.Date)).ToListAsync();

            Menu menuToSafe;
            for (int i = 0; i <= menuList.Count - 2; i++)
            {
                for (int j = 0; j <= menuList.Count - 2; j++)
                {
                    //Sorts list by Date
                    if (menuList[j].Date > menuList[j + 1].Date)
                    {
                        menuToSafe = menuList[j + 1];
                        menuList[j + 1] = menuList[j];
                        menuList[j] = menuToSafe;
                    }
                }
            }

            for (int i = 0; i <= menuList.Count - 2; i++)
            {
                for (int j = 0; j <= menuList.Count - 2; j++)
                {
                    if (menuList[j].WhichMenu > menuList[j + 1].WhichMenu)
                    {
                        menuToSafe = menuList[j + 1];
                        menuList[j + 1] = menuList[j];
                        menuList[j] = menuToSafe;
                    }
                }
            }

            return new JsonResult(menuList);
        }

        [HttpGet]
        [Route("menu/returnDateOfThursdayOfWeek")]
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

        /*
        [HttpGet]
        [Route("menu/getThisWeeklyMenu")]
        public async Task<IActionResult> AsyncGetThisWeeklyMenu()
        {
            DateTime dateTimeToday = DateTime.Now;

            //Gibt die Wochennummer aus 
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = dfi.Calendar;
            int weekOfYear = calendar.GetWeekOfYear(dateTimeToday, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            int year = DateTime.Now.Year;

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var resultDateTime = firstThursday.AddDays(weekNum * 7);

            DateOnly resultDateOnly = DateOnly.FromDateTime(resultDateTime);
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            List<DateOnly> days = new List<DateOnly>()
            {
                resultDateOnly.AddDays(-3), //Monday
                resultDateOnly.AddDays(-2), //Tuesday
                resultDateOnly.AddDays(-1), //Wednesday
                resultDateOnly,             //Thursday
                resultDateOnly.AddDays(+1), //Friday
                //resultDateOnly.AddDays(+2), //Saturday
                //resultDateOnly.AddDays(+3)  //Sunday

            };

            List<Menu> menuList = new List<Menu>();
            menuList = await this._context.Menues.Where(m => days.Contains(m.Date)).ToListAsync();

            Menu menuToSafe;
            for (int i = 0; i <= menuList.Count - 2; i++)
            {
                for (int j = 0; j <= menuList.Count - 2; j++)
                {
                    //Sorts list by Date
                    if (menuList[j].Date > menuList[j + 1].Date)
                    {
                        menuToSafe = menuList[j + 1];
                        menuList[j + 1] = menuList[j];
                        menuList[j] = menuToSafe;
                    }
                    if (menuList[j].WhichMenu > menuList[j + 1].WhichMenu && menuList[j].Date == menuList[j + 1].Date)
                    {
                        menuToSafe = menuList[j + 1];
                        menuList[j + 1] = menuList[j];
                        menuList[j] = menuToSafe;
                    }
                }
            }
            return new JsonResult(menuList);
        }   
        */

    }
}
