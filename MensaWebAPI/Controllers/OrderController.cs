using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MensaWebAPI.Controllers
{
    [Route("api/mensa")]
    [ApiController]
    public class OrderController : ControllerBase
    {
            private MenuContext _context = new MenuContext();

        public OrderController(MenuContext context) 
        {
                this._context = context;    
        }


        [HttpGet]
        [Route("order/getAll")]
        public async Task<IActionResult> AsyncGetAllOrders()
        {
            return new JsonResult(await this._context.Orders.ToListAsync());
        }

        [HttpGet]
        [Route("order/getOrderByUserEmail/{userMail}")]
        public async Task<IActionResult> AsyncGetOrderByUserEmail(String mail)
        {
            return new JsonResult(await this._context.Orders.Where(o => o.UserEmail.Equals(mail)).FirstAsync());
        }

        [HttpPost]
        [Route("order/safeOrder")]
        public async Task<IActionResult> AsyncSafeOrder(Order order)
        {
            this._context.Orders.Add(order);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }
    }
}
