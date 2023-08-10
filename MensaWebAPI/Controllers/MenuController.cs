using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MensaWebAPI.Controllers
{

    [Route("api/mensa")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private MenuContext _context = new MenuContext();


        public MenuController(MenuContext context) 
        {
            this._context = context;
        }


        [HttpGet]
        [Route("menu/getAll")]
        public async Task<IActionResult> AsyncGetAllMenues()
        {
            return new JsonResult(await this._context.Menues.ToListAsync());
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
            
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }

        [HttpGet]
        [Route("menu/getMenuByDate/{menuDate}")]
        public async Task<IActionResult> AsyncGetMenuByDate(DateOnly menuDate)
        {
            return new JsonResult(await this._context.Menues.Where(m => m.Date.Equals(menuDate)).ToListAsync());
        }

        [HttpGet]
        [Route("menu/getDailyMenu")]
        public async Task<IActionResult> AsyncGetDailyMenu()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            return new JsonResult(await this._context.Menues.Where(m => m.Date.Equals(today)).ToListAsync());
        }

        [HttpGet]
        [Route("menu/getWeeklyMenu")]
        public async Task<IActionResult> AsyncGetWeeklyMenu(DateTime dateTimeToday)
        {
            //dateTimeToday = DateTime.Now;

            //Gibt die Wochennummer aus 
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            int weekNr = cal.GetWeekOfYear(dateTimeToday, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            //Gibt den heuten Tag aus
            DateOnly dateToday = DateOnly.FromDateTime(dateTimeToday);
            return new JsonResult(await this._context.Menues.Where(m => m.Date.Equals(dateToday)).ToListAsync());

            //todo
            //aus der wochennummer alle tage der woche rausbekommen z.B: woche 32 -> 07.08.2023 (Montag) - 13.08.2023 (Sonntag)
            //die Menüs von jeden Tag ausgeben 
        }
    }
}
