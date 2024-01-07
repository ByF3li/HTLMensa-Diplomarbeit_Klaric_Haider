using MensaWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MensaAppKlassenBibliothek;
using System.Globalization;

namespace MensaWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuPersonAPIController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public MenuPersonAPIController(MenuContext context)
        {
            this._context = context;
        }

        [HttpGet("getAllOrders")]
        public async Task<IActionResult> AsyncGetAllOrders()
        {
            return new JsonResult(await this._context.MenuPersons.ToListAsync(), options);
        }

        [HttpGet("getAllOrderByUserEmail")]
        public async Task<IActionResult> AsyncGetAllOrderByUser(String mail)
        {
            var menus = await this._context.MenuPersons
                .Include(mp => mp.Menu)
                .ThenInclude(m => m.Prices)
                .Where(mp => mp.Person.Email == mail)
                .ToListAsync();

            return new JsonResult(menus, options);
        }

        [HttpPost("saveOrder")]
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

        [HttpDelete("deleteOrderByMenuId")]
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

        [HttpPut("updatePayedOrder")]
        public async Task<IActionResult> AsyncUpdatePayedOrder(string userEmail)
        {
            List<MenuPerson> shoppingCart = (this._context.MenuPersons.Where(mp => (mp.Person.Email.Equals(userEmail)) && (mp.InShoppingcart))).ToList();
            shoppingCart.ForEach(mp => {
                mp.Payed = true;
                mp.OrderDate = DateOnly.FromDateTime(DateTime.Now);
                mp.InShoppingcart = false;
            });
            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;
            return new JsonResult(shoppingCart, options);
        }

        [HttpPut("updateActivatedOrder")]
        public async Task<IActionResult> AsyncUpdateActivatedOrder(string userEmail)
        {
            List<MenuPerson> shoppingCart = (this._context.MenuPersons.Where(mp => (mp.Person.Email.Equals(userEmail)) && (mp.Payed) && !mp.Activated)).ToList();

            shoppingCart.ForEach(mp => mp.Activated = true);
            var isUpdateSuccessful = await this._context.SaveChangesAsync() > 0;
            return new JsonResult(shoppingCart, options);
        }
    }

}
