using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PayPalWebsiteTeil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalAPIController : Controller
    {
        [HttpPost("sendShoppingcartData")]
        public IActionResult AsyncSendShoppingcartData([FromBody] Shoppingcart shoppingcart)
        {
            TempData["Total"] = shoppingcart.Total;
            TempData["ProductIdentifiers"] = shoppingcart.ProductIdentifiers;
            TempData.Keep();

            return Ok(new { Message = "Data received successfully.", Data = shoppingcart });
        }

        [HttpPost("sendMessage")]
        public IActionResult AsyncSendMessage(string message)
        {
            return new JsonResult(message);
        }
    }
}
