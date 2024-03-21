using MensaWebsite.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MensaAppKlassenBibliothek;

namespace MensaWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonAPIController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public PersonAPIController(MenuContext context)
        {
            this._context = context;
        }

        [HttpGet("getAllPersons")]
        public async Task<IActionResult> AsyncGetAllPerson()
        {
            return new JsonResult(await this._context.Persons.ToListAsync(), options);
        }

        [HttpGet("getShoppingcart")]
        public async Task<IActionResult> AsyncGetShoppingcart(string email)
        {
            return new JsonResult((await this._context.MenuPersons.Where(mp => mp.Person.Email.Equals(email) && mp.InShoppingcart).ToListAsync()), options);
        }

        [HttpPost("addPerson")]
        public async Task<IActionResult> AsyncAddPerson(Person p)
        {
            Person personToFind = this._context.Persons.Find(p.Email);
            if (personToFind != null)
            {
                return new JsonResult("Success");
            }
            else
            {
                this._context.Persons.Add(p);
                return new JsonResult((await this._context.SaveChangesAsync()) == 1, options);
            }
        }

    }
}
