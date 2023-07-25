using Microsoft.AspNetCore.Mvc;

namespace MensaWebsite.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAllOrders()
        {
            return View();
        }
    }
}
