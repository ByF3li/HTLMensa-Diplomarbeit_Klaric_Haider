using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MensaAppKlassenBibliothek;
using System.Globalization;

namespace MensaWebAPI.Controllers
{
    [Route("api/mensa")]
    [ApiController]
    public class MenuPersonController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public MenuPersonController(MenuContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("order/getAllOrders")]
        public async Task<IActionResult> AsyncGetAllOrders()
        {
            return new JsonResult(await this._context.MenuPersons.ToListAsync(), options);
        }

        [HttpGet]
        [Route("order/getAllOrderByUserEmail")]
        public async Task<IActionResult> AsyncGetAllOrderByUser(String mail)
        {

            return new JsonResult(await this._context.MenuPersons.Where(mp => mp.Person.Email == mail).ToListAsync(), options);
        }

        [HttpGet]
        [Route("order/getAllOrderByUserEmailFromThisWeek")]
        public async Task<IActionResult> AsyncGetAllOrderByUserEmailFromThisWeek(String mail)
        {
            List<MenuPerson> mp = await this._context.MenuPersons.Where(mp => mp.Person.Email == mail).ToListAsync();


            List<MenuPerson> menupersons = new List<MenuPerson>();
            DateOnly resultDateOnly = ReturnThisWeek();

            foreach (MenuPerson menuperson in mp) 
            { 
                if((menuperson.OrderDate >= resultDateOnly.AddDays(-3)) && (menuperson.OrderDate <= resultDateOnly.AddDays(+1)))
                {
                    menupersons.Add(menuperson);
                }       
            }
            return new JsonResult(menupersons, options);
        }

        [HttpPost]
        [Route("order/saveOrder")]
        public async Task<IActionResult> AsyncSaveOrder(List<MenuPerson> shoppingcart)
        {
            List<MenuPerson> mp1 = new List<MenuPerson>();
            foreach(MenuPerson menuperson in shoppingcart)
            {
                MenuPerson? mp = await _context.MenuPersons.FirstOrDefaultAsync(mp => mp.MenuPersonId == menuperson.MenuPersonId);
                if(menuperson.MenuPersonId == 0)
                {
                    Person p = await _context.Persons.FindAsync(menuperson.Person.Email);
                    menuperson.Person = p;
                    Menu m = await _context.Menues.FindAsync(menuperson.Menu.MenuId);
                    menuperson.Menu = m;

                    await _context.MenuPersons.AddAsync(menuperson);
                }
            }

            return new JsonResult((await this._context.SaveChangesAsync()) >= 1);
        }

        [HttpDelete]
        [Route("order/deleteOrderByMenuId")]
        public async Task<IActionResult> AsyncDeleteOrderByMenuId(string userEmail, int menuId)
        {
            MenuPerson menupersons = (await this._context.MenuPersons.FirstOrDefaultAsync(o => o.Person.Email.Equals(userEmail) && o.Menu.MenuId == menuId));

            if (menupersons == null)
            {
                return NotFound($"Order with MenuId = {menuId} not found");
            }

            this._context.MenuPersons.Remove(menupersons);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }

        [HttpPut]
        [Route("order/updatePayedOrder")]
        public async Task<IActionResult> AsyncUpdatePayedOrder(string userEmail)
        {
            List<MenuPerson> shoppingCart = (this._context.MenuPersons.Where(mp => (mp.Person.Email.Equals(userEmail)) && (mp.InShoppingcart))).ToList();
            shoppingCart.ForEach(mp => {
                mp.Payed = true;
                mp.InShoppingcart = false;
            });
            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;
            return new JsonResult(shoppingCart, options);
        }

        [HttpPut]
        [Route("order/updateActivatedOrder")]
        public async Task<IActionResult> AsyncUpdateActivatedOrder(string userEmail)
        {
            List<MenuPerson> shoppingCart = (this._context.MenuPersons.Where(mp => (mp.Person.Email.Equals(userEmail)) && (mp.Payed) && !mp.Activated)).ToList();

            shoppingCart.ForEach(mp => mp.Activated = true);
            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;
            return new JsonResult(shoppingCart, options);
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
    }

}
