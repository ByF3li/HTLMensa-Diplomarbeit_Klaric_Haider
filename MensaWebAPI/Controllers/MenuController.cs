using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;
using Microsoft.VisualBasic;

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
            return new JsonResult(await this._context.Menues.ToListAsync(), options);
        }

        [HttpPost]
        [Route("menu/saveMenu")]
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

        [HttpGet]
        [Route("menu/getMenuByDate/{menuDate}")]
        public async Task<IActionResult> AsyncGetMenuByDate(DateOnly menuDate)
        {

            var toSortDate = await this._context.Menues.Where(m => m.Date.Equals(menuDate)).ToListAsync();
            toSortDate.Sort();
            return new JsonResult(toSortDate, options);
        }

        [HttpGet]
        [Route("menu/getMenuById/{menuId}")]
        public async Task<IActionResult> AsyncGetMenuById(int menuId)
        {
            return new JsonResult(await this._context.Menues.Where(m => m.MenuId.Equals(menuId)).FirstAsync(), options);
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

        [HttpPut]
        [Route("menu/changeMenuById")]
        public async Task<IActionResult> AsyncUpdateMenu(Menu updatedMenu)
        {
            /*
            if (updatedMenu == null)
            {
                return BadRequest("Invalid data provided");
            }
            */

            var existingMenu = await this._context.Menues.FindAsync(updatedMenu.MenuId);

            /*
            if (existingMenu == null)
            {
                return NotFound($"Menu with Id = {updatedMenu.MenuId} not found");
            }
            */

            existingMenu.Date = updatedMenu.Date;

            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;

            return new JsonResult(isUpdateSuccessful, options);
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
            };

            List<Menu> menuList = new List<Menu>();
            menuList = await this._context.Menues.Where(m => days.Contains(m.Date)).ToListAsync();
            
            List<Menu> sortedMenuList = SortMenus(menuList);

            return new JsonResult(sortedMenuList, options);
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
        public List<Menu> SortMenus(List<Menu> menuList)
        {
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

            return menuList;
        }
    
    }
}
