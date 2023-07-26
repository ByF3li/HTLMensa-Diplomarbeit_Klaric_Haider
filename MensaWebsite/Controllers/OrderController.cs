using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace MensaWebsite.Controllers
{
    public class OrderController : Controller
    {
        private List<Menu> menues = new();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllOrders()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShowAllOrders(DateTime orderDate)
        {

            try
            {
                getOrderByDate(orderDate);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View();
        }

        public async void getOrderByDate(DateTime orderDate)
        {
            // TODO: schauen wegen DateTime format funktioniert nicht es ist 18.7.2023; muss aber 2023-07-18 sein!!!!!
            HttpClient client = new HttpClient();
            menues = await client.GetFromJsonAsync<List<Menu>>("https://localhost:7286/api/mensa/menu/getMenuByDate/" + orderDate);
        }
    }
}
