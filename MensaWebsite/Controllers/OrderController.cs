using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace MensaWebsite.Controllers
{
    public class OrderController : Controller
    {
        List<Menu> menues = new();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllOrders()
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> ShowAllOrders(DateTime Date)
        {           
            HttpClient client = new HttpClient();
            try
            {
                menues = await client.GetFromJsonAsync<List<Menu>>("https://localhost:7286/api/mensa/menu/getMenuByDate/" + Date.ToString("yyyy'-'MM'-'dd"));
                foreach (Menu menu in menues)
                {
                    Console.WriteLine(menu);
                }
                await Console.Out.WriteLineAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(menues);
        }
    }
}
