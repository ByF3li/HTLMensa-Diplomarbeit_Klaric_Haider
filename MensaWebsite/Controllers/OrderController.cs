using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MensaWebsite.Controllers
{
    public class OrderController : Controller
    {
        List<Menu> menus = new();

        private MenuContext _context = new MenuContext();
        public OrderController(MenuContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ViewResult> ShowAllOrders()
        {
            try
            {
                menus = await _context.Menues.Include("Orders").Where(m => m.Date == DateOnly.FromDateTime(DateTime.Now)).OrderBy(m => m.WhichMenu).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(menus);
        }

        [HttpPost]
        public async Task<ViewResult> ShowAllOrders(DateOnly date)
        {   
            try
            {
                menus = await _context.Menues.Include("Orders").Where(m => m.Date == date).OrderBy(m => m.WhichMenu).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            // TODO: Daten werden übergeben aber nicht richtig angezeigt in der View
            return View(menus);
        }
    }
}
