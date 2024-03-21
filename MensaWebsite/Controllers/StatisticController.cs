using Microsoft.AspNetCore.Mvc;
using MensaAppKlassenBibliothek;
using MensaWebsite.Models.DB;
using Microsoft.EntityFrameworkCore;
using MensaWebsite.Models;


namespace MensaWebsite.Controllers
{
    public class StatisticController : Controller
    {
        private MenuContext _context = new MenuContext();
        public StatisticController(MenuContext context)
        {
            this._context = context;
        }


        public IActionResult showStatistic()
        {
            return View();
        }
        public IActionResult MenuChartsPartialView()
        {
            ViewBag.buttonClick = "Menü";
            return View("showStatistic");
        }
        public IActionResult OrderChartsPartialView()
        {
            ViewBag.buttonClick = "Order";
            return View("showStatistic");
        }


        [HttpGet]
        public async Task<JsonResult> getMostSoldMenus()
        {
            List<Menu> menus = new();

            try
            {
                menus = await _context.Menues.Include("MenuPersons").Include("Prices").Where(m => m.Date == DateOnly.FromDateTime(DateTime.Now)).OrderBy(m => m.Prices.PriceId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            var countMenus = new int[3]
            {
                menus[0].MenuPersons.Count,
                menus[1].MenuPersons.Count,
                menus[2].MenuPersons.Count
            };
            return new JsonResult(Ok(countMenus));
        }
        [HttpGet]
        public async Task<JsonResult> getSoldMenus(string monthlyOrWeekly)
        {
            List<Menu> menus = new();
            List<forChartDTO> dataForMenu1 = new();
            List<forChartDTO> dataForMenu2 = new();
            List<forChartDTO> dataForMenu3 = new();
            List<Menu> menu1 = new();
            List<Menu> menu2 = new();
            List<Menu> menu3 = new();
            List<List<forChartDTO>> dataForChart = new();
            try
            {
                if (monthlyOrWeekly == "monthly")
                {
                    menus = await _context.Menues.Include("Prices").Include("MenuPersons").Where(m => m.Date.Month == DateOnly.FromDateTime(DateTime.Now).Month).ToListAsync();
                }
                else if (monthlyOrWeekly == "weekly")
                {
                    menus = await _context.Menues.Include("Prices").Include("MenuPersons").Where(m => getWholeWeek().Contains(m.Date)).ToListAsync();
                }

                menu1 = menus.Where(m => m.Prices.PriceId == 1).OrderBy(m => m.Date).ToList();
                menu2 = menus.Where(m => m.Prices.PriceId == 2).OrderBy(m => m.Date).ToList();
                menu3 = menus.Where(m => m.Prices.PriceId == 3).OrderBy(m => m.Date).ToList();

                foreach (Menu m in menu1)
                {
                    forChartDTO d = new();
                    d.x = m.Date.ToString("dd.MM.yyyy");
                    d.y = m.MenuPersons.Count;
                    dataForMenu1.Add(d);
                }
                foreach (Menu m in menu2)
                {
                    forChartDTO d = new();
                    d.x = m.Date.ToString("dd.MM.yyyy");
                    d.y = m.MenuPersons.Count;
                    dataForMenu2.Add(d);
                }
                foreach (Menu m in menu3)
                {
                    forChartDTO d = new();
                    d.x = m.Date.ToString("dd.MM.yyyy");
                    d.y = m.MenuPersons.Count;
                    dataForMenu3.Add(d);
                }
                dataForChart = new()
                {
                    dataForMenu1,
                    dataForMenu2,
                    dataForMenu3
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return new JsonResult(Ok(dataForChart));
        }

        [HttpGet]
        public async Task<JsonResult> getSoldPickedUpMenu(int WhichMenu, string monthlyOrWeekly)
        {
            List<Menu> menuList = new();
            List<forChartDTO> notPickedUpMenus = new();
            List<forChartDTO> allOrderedMenus = new();
            List<List<forChartDTO>> listForChart = new();

            try
            {
                if (monthlyOrWeekly == "monthly")
                {
                    menuList = await _context.Menues.Include(m => m.MenuPersons).Include(m => m.Prices).Where(m => m.Prices.PriceId == WhichMenu && m.Date.Month == DateOnly.FromDateTime(DateTime.Now).Month).OrderBy(m => m.Date).ToListAsync();
                }
                else if (monthlyOrWeekly == "weekly")
                {
                    menuList = await _context.Menues.Include("Prices").Include("MenuPersons").Where(m => m.Prices.PriceId == WhichMenu && getWholeWeek().Contains(m.Date)).ToListAsync();
                }

                foreach(Menu m in menuList)
                {
                    forChartDTO forChartDTO = new();
                    forChartDTO.x = m.Date.ToString("dd.MM.yyyy");
                    foreach (MenuPerson mp in m.MenuPersons)
                    {
                        if(mp.Activated)
                        {
                            forChartDTO.y += 1;
                        }
                    }
                    notPickedUpMenus.Add(forChartDTO);
                }
                foreach (Menu m in menuList)
                {
                    forChartDTO forChartDTO = new();
                    forChartDTO.x = m.Date.ToString("dd.MM.yyyy");
                    forChartDTO.y = m.MenuPersons.Count;
                    allOrderedMenus.Add(forChartDTO);
                }

                listForChart = new()
                {
                    notPickedUpMenus,
                    allOrderedMenus
                };


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return new JsonResult(Ok(listForChart));
        }

        public List<DateOnly> getWholeWeek()
        {
            DateOnly dateValue = DateOnly.FromDateTime(DateTime.Now);
            int dayOfWeek = (int)dateValue.DayOfWeek - 1;
            DateOnly firstOfTheWeek = dateValue.AddDays(-dayOfWeek);
            List<DateOnly> thisWeek = new()
                    {
                        firstOfTheWeek,
                        firstOfTheWeek.AddDays(1),
                        firstOfTheWeek.AddDays(2),
                        firstOfTheWeek.AddDays(3),
                        firstOfTheWeek.AddDays(4),
                        firstOfTheWeek.AddDays(5),
                        firstOfTheWeek.AddDays(6)
                    };
            return thisWeek;
        }


    }
}
