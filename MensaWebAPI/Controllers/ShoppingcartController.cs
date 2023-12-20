using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Text.Json;
using MensaAppKlassenBibliothek;

namespace MensaWebAPI.Controllers
{
    [Route("api/mensa")]
    [ApiController]
    public class ShoppingcartController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        public ShoppingcartController(MenuContext context)
        {
            this._context = context;
        }

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        [HttpGet]
        [Route("shoppingcart/getAll")]
        public async Task<IActionResult> AsyncGetAllShoppingcarts()
        {
            return new JsonResult(await this._context.ShoppingCarts.ToListAsync(), options);
        }

        [HttpGet]
        [Route("shoppingcart/getShoppingcartById/{shoppingcartId}")]
        public async Task<IActionResult> AsyncGetShoppingcartById(int shoppingcartId)
        {
            return new JsonResult(await this._context.ShoppingCarts.Where(m => m.ShoppingCartId.Equals(shoppingcartId)).FirstAsync(), options);
        }

        [HttpDelete]
        [Route("shoppingcart/deleteShoppingcartById/{shoppingcartId}")]
        public async Task<IActionResult> AsyncDeleteShoppingcartById(int shoppingcartId)
        {
            var shoppingcartToDelete = await this._context.ShoppingCarts.FindAsync(shoppingcartId);

            if (shoppingcartToDelete == null)
            {
                return NotFound($"Article with Id = {shoppingcartId} not found");
            }

            this._context.ShoppingCarts.Remove(shoppingcartToDelete);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);

        }

        [HttpPost]
        [Route("shoppingcart/saveShoppingcart")]
        public async Task<IActionResult> AsyncSafeShoppingcart(int id, List<MenuShoppingCartItem> menus)
        {
            ShoppingCart sc = new ShoppingCart()
            {
                ShoppingCartId = id,
                MenuItems = menus
            };
            this._context.ShoppingCarts.Add(sc);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1, options);
        }
    }
}
