using MensaAppKlassenBibliothek;
using MensaWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Connections.Features;
using MensaWebsite.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MensaWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuAPIController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public MenuAPIController(MenuContext context)
        {
            this._context = context;
        }


        [HttpGet("getAllMenus")]
        public async Task<IActionResult> AsyncGetAllMenues()
        {
            return new JsonResult(await this._context.Menues.Include("Prices").ToListAsync(), options);
        }

        [HttpPost("saveMenu")]
        public async Task<IActionResult> AsyncSafeMenu(int whichMenu, string starter, string mainCourse, DateOnly date)
        {
            Menu menu = new Menu()
            {
                Prices = await _context.Prices.FindAsync(whichMenu),
                Starter = starter,
                MainCourse = mainCourse,
                Date = date
            };
            this._context.Menues.Add(menu);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1, options);
        }

        [HttpGet("getMenuByDate/{menuDate}")]
        public async Task<IActionResult> AsyncGetMenuByDate(DateOnly menuDate)
        {
            var toSortDate = await this._context.Menues.Include("Prices").Where(m => m.Date.Equals(menuDate)).ToListAsync();
            toSortDate.Sort();
            return new JsonResult(toSortDate, options);
        }

        [HttpGet("getMenuById/{menuId}")]
        public async Task<IActionResult> AsyncGetMenuById(int menuId)
        {
            return new JsonResult(await this._context.Menues.Include("Prices").Where(m => m.MenuId.Equals(menuId)).FirstAsync(), options);
        }

        [HttpDelete("deleteMenuById/{menuId}")]
        public async Task<IActionResult> AsyncDeleteMenuById(int menuId)
        {
            var articleToDelete = await this._context.Menues.FindAsync(menuId);

            if (articleToDelete == null)
            {
                return NotFound($"Article with Id = {menuId} not found");
            }

            this._context.Menues.Remove(articleToDelete);
            return new JsonResult((await this._context.SaveChangesAsync()) >= 1);

        }

        [HttpGet("getDailyMenu")]
        public async Task<IActionResult> AsyncGetDailyMenu()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            return new JsonResult(await this._context.Menues.Include("Prices").Where(m => m.Date.Equals(today)).ToListAsync());
        }

        [HttpPut("changeMenuById")]
        public async Task<IActionResult> AsyncUpdateMenu(int menuToUpdateId, int? whichMenu, string? starter, string? mainCourse, DateOnly? date)
        {
            if (menuToUpdateId == null)
            {
                return BadRequest("Invalid data provided");
            }
            
            var existingMenu = await this._context.Menues.FindAsync(menuToUpdateId);

            if (existingMenu == null)
            {
                return NotFound($"Menu with Id = {menuToUpdateId} not found");
            }
            
            if(whichMenu != null) { existingMenu.Prices = await _context.Prices.FindAsync(whichMenu); } 
            if(starter != null) { existingMenu.Starter = starter; }
            if (mainCourse != null) { existingMenu.MainCourse = mainCourse; }
            if (date != null) { existingMenu.Date = (DateOnly)date; }

            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;
            
            if (isUpdateSuccessful)
            {
                return new JsonResult(existingMenu, options);
            }
            else
            {
                return NotFound($"Updating Menu with Id = {menuToUpdateId} not successful");
            }
        }

        [HttpGet("getThisWeeklyMenu")]
        public async Task<IActionResult> AsyncGetThisWeeklyMenu()
        {
            DateOnly resultDateOnly = ReturnThisWeek();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (today >= resultDateOnly.AddDays(+2))
            {
                resultDateOnly = resultDateOnly.AddDays(+7);
            }

            List<DateOnly> days = new List<DateOnly>()
            {
                resultDateOnly.AddDays(-3), //Monday
                resultDateOnly.AddDays(-2), //Tuesday
                resultDateOnly.AddDays(-1), //Wednesday
                resultDateOnly,             //Thursday
                resultDateOnly.AddDays(+1), //Friday
            };

            List<Menu> menuList = new List<Menu>();
            menuList = await this._context.Menues.Include("Prices").Where(m => days.Contains(m.Date)).ToListAsync();

            List<Menu> sortedMenuList = SortMenus(menuList);

            return new JsonResult(sortedMenuList, options);
        }

        [HttpGet("getPriceForMenu")]
        public async Task<IActionResult> AsyncGetPriceForMenu(int menuId)
        {
            var menuToGetPriceFor = await this._context.Menues.FindAsync(menuId);
            if(menuToGetPriceFor != null)
            {
                PriceForMenu price = await _context.Prices.FindAsync(menuToGetPriceFor.Prices.PriceId);
                return new JsonResult(price, options);

            }
            else
            {
                return BadRequest("No menu found to get price for");
            }
        }

        private DateOnly ReturnThisWeek()
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
        private List<Menu> SortMenus(List<Menu> menuList)
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
                    if (menuList[j].Date == menuList[j + 1].Date && menuList[j].Prices.PriceId > menuList[j + 1].Prices.PriceId)
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
