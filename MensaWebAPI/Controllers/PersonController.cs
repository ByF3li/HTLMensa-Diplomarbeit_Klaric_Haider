using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MensaAppKlassenBibliothek;

namespace MensaWebAPI.Controllers
{
    [Route("api/mensa")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public PersonController(MenuContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("person/getAllPersons")]
        public async Task<IActionResult> AsyncGetAllPerson()
        {
            return new JsonResult(await this._context.Persons.ToListAsync(), options);
        }

        [HttpGet]
        [Route("person/getShoppingcart")]
        public async Task<IActionResult> AsyncGetShoppingcart(string email)
        {
            return new JsonResult((await this._context.MenuPersons.Where(mp => mp.Person.Email.Equals(email) && mp.InShoppingcart).ToListAsync()), options);
        }
    }
}
