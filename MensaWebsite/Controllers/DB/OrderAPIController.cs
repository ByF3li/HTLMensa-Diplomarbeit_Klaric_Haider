using MensaAppKlassenBibliothek;
using MensaWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MensaWebsite.Controllers.DB
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
            private MenuContext _context = new MenuContext();

        public OrderAPIController(MenuContext context) 
        {
                this._context = context;    
        }
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };


        [HttpGet("getAllOrders")]
        public async Task<IActionResult> AsyncGetAllOrders()
        {
            return new JsonResult(await this._context.Orders.Include("Menus").ToListAsync(),options);
        }

        [HttpPost("saveOrder/{order}")]
        public async Task<IActionResult> AsyncSafeOrder(Order order)
        {
            this._context.Orders.Add(order);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }
    }
}
