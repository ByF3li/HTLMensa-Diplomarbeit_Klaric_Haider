using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;

namespace MensaWebsite.Controllers
{
    public class MenuController : Controller
    {

        HttpResponseMessage responseMessage = new();
        List<Menu> menus = new List<Menu>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SafeMenues()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SafeMenues(int whichMenu, string starter, string mainCourse, decimal price, DateTime date)
        {
            Menu menu = new Menu()
            {
                WhichMenu = whichMenu,
                Starter = starter,
                MainCourse = mainCourse,
                Price = price,
                Date = date
            };


            try
            {
                HttpClient client = new HttpClient();
                responseMessage = await client.PostAsJsonAsync<Menu>("https://localhost:7286/api/mensa/menu/safeMenu", menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            



            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessAlert"] = "Menü wurde erfolgreich hinzugefügt!";
                return RedirectToAction("SafeMenues");
            }

            return View();
        }
        public async Task<ViewResult> ShowAllMenus()
        {
            HttpClient client = new HttpClient();
            try
            {
                menus = await client.GetFromJsonAsync<List<Menu>>("https://localhost:7286/api/mensa/menu/getAll");
                foreach (Menu menu in menus)
                {
                    Console.WriteLine(menu);
                }
                await Console.Out.WriteLineAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            return View(menus);
        }
    }
}
