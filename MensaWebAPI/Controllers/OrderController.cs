using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MensaWebAPI.Controllers
{
    [Route("api/mensa")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        private MenuContext _context = new MenuContext();

        public OrderController(MenuContext context) 
        {
                this._context = context;    
        }


        [HttpGet]
        [Route("order/getAll")]
        public async Task<IActionResult> AsyncGetAllOrders()
        {
            return new JsonResult(await this._context.Orders.ToListAsync(), options);
        }

        [HttpGet]
        [Route("order/getOrderByUserEmail")]
        public async Task<IActionResult> AsyncGetOrderByUserEmail(String mail)
        {
            return new JsonResult(await this._context.Orders.Where(o => o.UserEmail.Equals(mail)).ToListAsync(), options);
        }

        [HttpPost]
        [Route("order/saveOrder")]
        public async Task<IActionResult> AsyncSaveOrder(DtoOrder order)
        {
            List<Menu> menus = new List<Menu>();

            foreach (int i in order.MenuIds)
            {
                Menu toAdd = await getMenuById(i);
                menus.Add(toAdd);
            }

            Order orderToPost = new Order()
            {
                OrderDate = order.OrderDate,
                UserEmail = order.UserEmail,
                Menus = menus
            };

            _context.Add(orderToPost);
            return new JsonResult((await this._context.SaveChangesAsync()) >= 1);
        }

        private async Task<Menu> getMenuById(int id)
        {
            return await _context.Menues.FindAsync(id);
        }

    }
}
