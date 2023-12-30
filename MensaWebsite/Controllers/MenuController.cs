using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using MensaWebsite.Models.DB;
using Microsoft.EntityFrameworkCore;
using MensaWebsite.Models;

namespace MensaWebsite.Controllers
{
    public class MenuController : Controller
    {
        List<Menu> menus = new List<Menu>();

        private MenuContext _context = new MenuContext();
        public MenuController(MenuContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<PartialViewResult> _PricesPartialView(int priceId)
        {
            MenuDTO menuDTO = new MenuDTO();
            PriceForMenu price = await _context.Prices.FindAsync(priceId);
            menuDTO.PriceStudent = price.PriceStudent;
            menuDTO.PriceTeacher = price.PriceTeacher;
            return PartialView(menuDTO);
        }
        public async Task<IActionResult> SafeMenues()
        {
            int priceId = 1;
            MenuDTO menuDTO = new MenuDTO();
            PriceForMenu price = await _context.Prices.FindAsync(priceId);
            menuDTO.PriceStudent = price.PriceStudent;
            menuDTO.PriceTeacher = price.PriceTeacher;
            menuDTO.Date = DateOnly.FromDateTime(DateTime.Now);
            return View(menuDTO);
        }

        [HttpPost]
        public async Task<IActionResult> SafeMenues(MenuDTO menuDTO)
        {
            bool isSuccess = false;
            if (menuDTO.WhichMenu < 1 || menuDTO.WhichMenu > 3)
            {
                ModelState.AddModelError("WhichMenu", "Es gibt nur Menüs von 1 bis 3!");
            }
            if (menuDTO.Starter == null || menuDTO.Starter.Trim().Length < 3)
            {
                ModelState.AddModelError("Starter", "Vorspeiße muss mehr als 3 Zeichen enthalten!");
            }
            if (menuDTO.MainCourse == null || menuDTO.MainCourse.Trim().Length < 3)
            {
                ModelState.AddModelError("MainCourse", "Hauptspeiße muss mehr als 3 Zeichen enthalten!");
            }
            if (menuDTO.Date < DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("Date", "Datum darf nicht in der Vergangenheit liegen!");
            }


            if(ModelState.IsValid)
            {
                try
                {
                    Menu menu = new Menu()
                    {
                        Prices = await _context.Prices.FindAsync(menuDTO.WhichMenu),
                        Starter = menuDTO.Starter,
                        MainCourse = menuDTO.MainCourse,
                        Date = menuDTO.Date
                    };
                    _context.Menues.Add(menu);
                    isSuccess = (await _context.SaveChangesAsync()) == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }


                if (isSuccess)
                {
                    TempData["SuccessAlert"] = "Menü wurde erfolgreich hinzugefügt!";
                    return RedirectToAction("SafeMenues");
                }
                else if (!isSuccess)
                {
                    TempData["NoSuccessAlert"] = "Menü konnte nicht gespeichert werden!";
                    return RedirectToAction("SafeMenues");
                }
            }

            return View(menuDTO);
        }
        public async Task<IActionResult> ShowAllMenus()
        {
            List<MenuDTO> menuDTOList = new List<MenuDTO>();
            try
            {
                menus = await _context.Menues.Include("Prices").OrderByDescending(m => m.MenuId).ToListAsync();
                foreach(Menu menu in menus)
                {
                    MenuDTO menuDTO = new();
                    menuDTO.MenuId = menu.MenuId;
                    menuDTO.WhichMenu = menu.Prices.PriceId;
                    menuDTO.Starter = menu.Starter;
                    menuDTO.MainCourse = menu.MainCourse;
                    menuDTO.Date = menu.Date;
                    menuDTO.PriceStudent = menu.Prices.PriceStudent;
                    menuDTO.PriceTeacher = menu.Prices.PriceTeacher;

                    menuDTOList.Add(menuDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View(menuDTOList);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMenuFromDatabase(int Id)
        {
            bool isSuccess = false;
            List<MenuDTO> menuDTOList = new List<MenuDTO>();

            try
            {
                var menuToDelete = await _context.Menues.FindAsync(Id);
                _context.Menues.Remove(menuToDelete);
                isSuccess = (await _context.SaveChangesAsync()) == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (isSuccess)
            {
                TempData["SuccessAlert"] = "Menü wurde erfolgreich gelöscht!";
                return PartialView("_MessagePartialView");
            }
            else
            {
                TempData["NoSuccessAlert"] = "Menü konnte nicht gelöscht werden!";
                return PartialView("_MessagePartialView");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMenu(int Id)
        {
            Menu menu = new();
            try
            {
               menu = await _context.Menues.FindAsync(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            MenuDTO menuDTO = new MenuDTO()
            {
                WhichMenu = menu.Prices.PriceId,
                Starter = menu.Starter,
                MainCourse = menu.MainCourse,
                Date = menu.Date,
                PriceStudent = menu.Prices.PriceStudent,
                PriceTeacher = menu.Prices.PriceTeacher
            };

            return View(menuDTO);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenu(int id, MenuDTO menuDTO)
        {
            bool isSuccess = false;
            if (menuDTO.WhichMenu < 1 || menuDTO.WhichMenu > 3)
            {
                ModelState.AddModelError("WhichMenu", "Es gibt nur Menüs von 1 bis 3!");
            }
            if (menuDTO.Starter == null || menuDTO.Starter.Trim().Length < 3)
            {
                ModelState.AddModelError("Starter", "Vorspeiße muss mehr als 3 Zeichen enthalten!");
            }
            if (menuDTO.MainCourse == null || menuDTO.MainCourse.Trim().Length < 3)
            {
                ModelState.AddModelError("MainCourse", "Hauptspeiße muss mehr als 3 Zeichen enthalten!");
            }

            if (ModelState.IsValid) {
                try
                {
                    var menuToEdit = _context.Menues.FirstOrDefault(m => m.MenuId == id);
                    if (menuToEdit != null) {
                        menuToEdit.MenuId = id;
                        menuToEdit.Prices = await _context.Prices.FindAsync(menuDTO.WhichMenu);
                        menuToEdit.Starter = menuDTO.Starter;
                        menuToEdit.MainCourse = menuDTO.MainCourse;
                        menuToEdit.Date = menuDTO.Date;
                    }
                    isSuccess = (await _context.SaveChangesAsync()) == 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (isSuccess)
                {
                    TempData["SuccessAlert"] = "Menü wurde erfolgreich bearbeitet!";
                    return RedirectToAction("EditMenu");
                }
                else if (!isSuccess)
                {
                    TempData["NoSuccessAlert"] = "Menü konnte nicht bearbeitet werden!";
                    return RedirectToAction("EditMenu");
                }
            }
            return View();
        }

        public async Task<IActionResult> UpdatePrice()
        {
            PriceForMenu p = await _context.Prices.FindAsync(1);
            return View(p);
        }
        [HttpGet]
        public async Task<PartialViewResult> _ChangePricePartialView(int priceId)
        {
            PriceForMenu p = await _context.Prices.FindAsync(priceId);
            return PartialView(p);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePrice(PriceForMenu price)
        {
            bool isSuccess = false;
            if(price.PriceId < 1 || price.PriceId > 3)
            {
                ModelState.AddModelError("PriceId", "Menü gibts nur zwischen 1 und 3!");
            }
            if (price.PriceStudent <= 0)
            {
                ModelState.AddModelError("PriceStudent", "Preis muss größer als 0 sein!");
            }
            if (price.PriceTeacher <= 0)
            {
                ModelState.AddModelError("PriceTeacher", "Preis muss größer als 0 sein!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    PriceForMenu p = await _context.Prices.FindAsync(price.PriceId);

                    if (p != null) {
                        p.PriceStudent = price.PriceStudent;
                        p.PriceTeacher = price.PriceTeacher;
                    }
                    isSuccess = (await _context.SaveChangesAsync() == 1);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (isSuccess)
                {
                    TempData["SuccessAlert"] = "Preis wurde erfolgreich bearbeitet!";
                    return RedirectToAction("UpdatePrice");
                }
                else if (!isSuccess)
                {
                    TempData["NoSuccessAlert"] = "Preis konnte nicht bearbeitet werden!";
                    return RedirectToAction("UpdatePrice");
                }
            }

            return View();
        }
    }

}