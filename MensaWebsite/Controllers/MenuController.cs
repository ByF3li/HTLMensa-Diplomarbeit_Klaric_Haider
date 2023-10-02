﻿using MensaAppKlassenBibliothek;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;

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
        public async Task<IActionResult> SafeMenues(int whichMenu, string starter, string mainCourse, decimal price, DateOnly date)
        {
            Menu menu = new Menu()
            {
                WhichMenu = whichMenu,
                Starter = starter,
                MainCourse = mainCourse,
                Price = price,
                Date = date,
                Orders = new List<Order>()
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
            }else if (!responseMessage.IsSuccessStatusCode)
            {
                TempData["NoSuccessAlert"] = "Menü konnte nicht gespeichert werden!";
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


        [HttpPost]
        public async Task<ViewResult> DeleteMenuFromDatabase(int Id)
        {        
            try
            {
                HttpClient client = new HttpClient();
                responseMessage = await client.DeleteAsync("https://localhost:7286/api/mensa/menu/deleteMenuById/" + Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditMenu(int Id)
        {
            Menu menu = new();
            try
            {
                HttpClient client = new HttpClient();
                menu = await client.GetFromJsonAsync<Menu>("https://localhost:7286/api/mensa/menu/getMenuById/" + Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View("SafeMenues", menu);
        }

        // TODO: speichern geht noch nicht! funktioniert aber in WebApi

        [HttpPost]
        public async Task<IActionResult> EditMenu(int id, int whichMenu, string starter, string mainCourse, decimal price, DateOnly date)
        {
            Menu menu = new Menu()
            {
                MenuId = id,
                WhichMenu = whichMenu,
                Starter = starter,
                MainCourse = mainCourse,
                Price = price,
                Date = date
            };

            try
            {
                HttpClient client = new HttpClient();
                responseMessage = await client.PatchAsJsonAsync<Menu>("https://localhost:7286/api/mensa/menu/editMenu", menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["SuccessAlert"] = "Menü wurde erfolgreich bearbeitet!";
                return RedirectToAction("SafeMenues");
            }
            else if (!responseMessage.IsSuccessStatusCode)
            {
                TempData["NoSuccessAlert"] = "Menü konnte nicht bearbeitet werden!";
                return RedirectToAction("SafeMenues");
            }

            return View();
        }

    }
}
