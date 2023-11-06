using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using MensaWebAPI.Models.DB;
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

        public IActionResult SafeMenues()
        {
            return View();
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
            if (menuDTO.Price < 0)
            {
                ModelState.AddModelError("Price", "Preis muss größer als 0 sein!");
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
                        WhichMenu = menuDTO.WhichMenu,
                        Starter = menuDTO.Starter,
                        MainCourse = menuDTO.MainCourse,
                        Price = menuDTO.Price,
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
        public async Task<ViewResult> ShowAllMenus()
        {
            try
            {
                menus = await _context.Menues.Include("Orders").ToListAsync();
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


        [HttpPost]
        public async Task<IActionResult> DeleteMenuFromDatabase(int Id)
        {
            bool isSuccess = false;

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
                return RedirectToAction("ShowAllMenus");
            }
            else if (!isSuccess)
            {
                TempData["NoSuccessAlert"] = "Menü konnte nicht gelöscht werden!";
                return RedirectToAction("ShowAllMenus");
            }


            return View("ShowAllMenus");
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
                WhichMenu = menu.WhichMenu,
                Starter = menu.Starter,
                MainCourse = menu.MainCourse,
                Price = menu.Price,
                Date = menu.Date
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
            if (menuDTO.Price < 0)
            {
                ModelState.AddModelError("Price", "Preis muss größer als 0 sein!");
            }

            if (ModelState.IsValid) {
                try
                {
                    var menuToEdit = _context.Menues.FirstOrDefault(m => m.MenuId == id);
                    if (menuToEdit != null) {
                        menuToEdit.MenuId = id;
                        menuToEdit.WhichMenu = menuDTO.WhichMenu;
                        menuToEdit.Starter = menuDTO.Starter;
                        menuToEdit.MainCourse = menuDTO.MainCourse;
                        menuToEdit.Price = menuDTO.Price;
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

    }
}
