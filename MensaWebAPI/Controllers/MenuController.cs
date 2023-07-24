using MensaAppKlassenBibliothek;
using MensaWebAPI.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MensaWebAPI.Controllers
{

    [Route("api/mensa")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private MenuContext _context = new MenuContext();


        public MenuController(MenuContext context) 
        {
            this._context = context;
        }


        [HttpGet]
        [Route("menu/getAll")]
        public async Task<IActionResult> AsyncGetAllMenues()
        {
            return new JsonResult(await this._context.Menues.ToListAsync());
        }

        [HttpPost]
        [Route("menu/safeMenu")]
        public async Task<IActionResult> AsyncSafeMenu(Menu menu)
        {
            this._context.Menues.Add(menu);
            return new JsonResult((await this._context.SaveChangesAsync()) == 1);
        }

    }
}
