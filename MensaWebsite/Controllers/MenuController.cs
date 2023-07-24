using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel;
using System.Threading.Tasks.Dataflow;

namespace MensaWebsite.Controllers
{
    public class MenuController : Controller
    {

        HttpResponseMessage responseMessage = new();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SafeMenues()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SafeMenues(int whichMenu, string starter, string mainCourse, decimal price, DateTime date)
        {
            Menu menu = new Menu()
            {
                WhichMenu = whichMenu,
                Starter = starter,
                MainCourse = mainCourse,
                Price = price,
                Date = date
            };

            SafeMenuInDatabase(menu);

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessAlert"] = "Menü wurde erfolgreich hinzugefügt!";
                return RedirectToAction("SafeMenues");
            }

            return View();
        }

        public async void SafeMenuInDatabase(Menu menu)
        {
                HttpClient client = new HttpClient();
                responseMessage = await client.PostAsJsonAsync<Menu>("https://localhost:7286/api/mensa/menu/safeMenu", menu);
        }

    }
}
