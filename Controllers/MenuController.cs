using ASP_PROJECT.DAL.CDAL;
using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.POCO;
using ASP_PROJECT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Controllers
{
    public class MenuController : Controller {
        private readonly IMenuDAL _menuDAL;

        public MenuController(IMenuDAL menuDAL) {
            _menuDAL = menuDAL;
        }

        public IActionResult Index() {
            // clear  va vider TOUTE la session, pas adapté 
            HttpContext.Session.SetString("DishesId", "");
            // chem absolu ! arch du syst
            return View("Views/Menu/Index.cshtml");
        }

        public IActionResult Delete(bool dishOrMenu, int menuID, int dishID) {
            if (dishOrMenu == true) {
                Menu SearchedMenu = Menu.GetMenuById(menuID, _menuDAL);
                SearchedMenu.Delete(_menuDAL);
                int restoId = (int)HttpContext.Session.GetInt32("restaurantId");
                return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = restoId });
            } else {
                Dish SearchedDish = Dish.GetDishById(dishID, _menuDAL);
                Restaurant r = new Restaurant();
                r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
                SearchedDish.Delete(_menuDAL);
                return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = r.Id });
            }
        }
        // Va permettre d'afficher au premier passage les infos préremplies dans les formulaires adéquats et les plats contenus dans les menus
        public IActionResult Update(bool dishOrMenu, int menuID, int dishID) {
            MenuAndDishViewModel vm = new MenuAndDishViewModel();
            vm.DishOrMenu = dishOrMenu;
            if (dishOrMenu == true) {
                string id = "";
                Menu searchedMenu = Menu.GetMenuById(menuID, _menuDAL);
                vm.Menu = searchedMenu;
                // Remplir la session
                HttpContext.Session.SetInt32("MenuId", vm.Menu.Id);
                HttpContext.Session.SetString("DishesId", "");

                string sessionsDishid = "";
                foreach (var dish in vm.Menu.DishList) {
                    if (sessionsDishid == "") {
                        id = dish.Id.ToString();
                        sessionsDishid += id;
                    } else {
                        id = dish.Id.ToString();
                        sessionsDishid += ";" + id;
                    }
                }
                Restaurant r = new Restaurant();
                r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
                vm.Dlist = r.GetDishes(_menuDAL);
                HttpContext.Session.SetString("DishesId", sessionsDishid);

            } else {
                Dish dishToModify = Dish.GetDishById(dishID, _menuDAL);
                vm.Dish = dishToModify;
                TempData["ModifyDishId"] = vm.Dish.Id;
            }
            return View("Update", vm);
        }
        public IActionResult Add(bool dishOrMenu) {
            MenuAndDishViewModel vm = new MenuAndDishViewModel();
            vm.DishOrMenu = dishOrMenu;

            if (dishOrMenu == true) {
                HttpContext.Session.SetString("DishesId", "");
                Restaurant r = new Restaurant();
                r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
                vm.Dlist = r.GetDishes(_menuDAL);
                return View("Add", vm);
            } else{
                Restaurant r = new Restaurant();
                r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
                return View("Add", vm);
            }
        }
        //POST METHODS


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(MenuAndDishViewModel vm) {
            Meal tryMenu = new Menu();
            Meal tryDish = new Dish();
            vm.Restaurant.Id = (int)HttpContext.Session.GetInt32("restaurantId");

            tryMenu = vm.Menu;
            tryDish = vm.Dish;

            if (vm.DishOrMenu == true) {
                bool success = tryMenu.Update(_menuDAL);

                if (success == true) {
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                } else {
                    TempData["ErreurAjout"] = "Menu non mis à jour";
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                }
            } else {
                bool success = tryDish.Update(_menuDAL);
                if (success == true) {
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                } else {
                    TempData["ErreurAjout"] = "Plat non mis à jour";
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(MenuAndDishViewModel vm) {
            Meal tryMenu = new Menu();
            Meal tryDish = new Dish();
            vm.Restaurant.Id= (int)HttpContext.Session.GetInt32("restaurantId");

            tryMenu = vm.Menu;
            tryDish = vm.Dish;
            if (vm.DishOrMenu == true) {
                bool success=tryMenu.Add(_menuDAL, vm.Restaurant);
                if (success == true) {
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                } else {
                    TempData["ErreurAjout"] = "Menu non ajouté";
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                }
            } else { 
                bool success = tryDish.Add(_menuDAL, vm.Restaurant);
                if (success == true) {
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                } else {
                    TempData["ErreurAjout"] ="Plat non ajouté";
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = vm.Restaurant.Id });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDishToMenu(int DishId, Menu menu, string operation) {
            MenuAndDishViewModel vm = new MenuAndDishViewModel();
            if (HttpContext.Session.GetString("DishesId") != null && HttpContext.Session.GetString("DishesId") != "") {
                string sessionIds = HttpContext.Session.GetString("DishesId");
                sessionIds += ";" + DishId.ToString();
                HttpContext.Session.SetString("DishesId", sessionIds);
                sessionIds = HttpContext.Session.GetString("DishesId");
                string[] idSplited = sessionIds.Split(";");
                foreach (var item in idSplited) {
                    int id = Int32.Parse(item);
                    Dish AddedDish = Dish.GetDishById(id, _menuDAL);
                    menu.DishList.Add(AddedDish);
                }

            } else {
                Dish AddedDish = Dish.GetDishById(DishId, _menuDAL);
                menu.DishList.Add(AddedDish);
                HttpContext.Session.SetString("DishesId", DishId.ToString());
            }
            vm.Menu = menu;
            Restaurant r = new Restaurant();
            r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
            vm.Dlist = r.GetDishes(_menuDAL);
            vm.DishOrMenu = true;
            if (operation == "modifying") {
                return View("Update", vm);
            } else {
                return View("Add", vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDishFromMenu(int DishId, Menu menu, string operation) {
            MenuAndDishViewModel vm = new MenuAndDishViewModel();

            if (HttpContext.Session.GetString("DishesId") != null && HttpContext.Session.GetString("DishesId") != "") {
                int flag = 0;
                menu.DishList = new List<Dish>();
                string sessionIds = HttpContext.Session.GetString("DishesId");
                sessionIds = HttpContext.Session.GetString("DishesId");
                string[] idSplited = sessionIds.Split(";");
                sessionIds = "";
                foreach (var item in idSplited) {
                    int id = Int32.Parse(item);
                    if (DishId != id) {
                        if (sessionIds != "") {
                            sessionIds += ";" + item;
                        }
                        if (sessionIds == "") {
                            sessionIds += item;
                        }
                    }
                    if (DishId == id && flag == 1) {
                        if (sessionIds != "") {
                            sessionIds += ";" + item;
                        }
                        if (sessionIds == "") {
                            sessionIds += item;
                        }
                        flag = 2;
                    }
                    //Permet de gerer le cas ou on voudrait supprimer un doublon d'un dish déjà ajouté
                    if (DishId == id && flag == 0) {
                        flag = 1;
                    }
                }
                if (sessionIds == "") {
                    menu.DishList = new List<Dish>();
                    HttpContext.Session.SetString("DishesId", "");

                } else {
                    HttpContext.Session.SetString("DishesId", sessionIds);
                    idSplited = sessionIds.Split(";");
                    foreach (var item in idSplited) {
                        int id = Int32.Parse(item);
                        Dish AddedDish = Dish.GetDishById(id, _menuDAL);
                        menu.DishList.Add(AddedDish);
                    }
                }
            }
            vm.Menu = menu;
            Restaurant r = new Restaurant();
            r.Id = (int)HttpContext.Session.GetInt32("restaurantId");
            vm.DishOrMenu = true;
            vm.Dlist = r.GetDishes(_menuDAL);
            if (operation == "modifying") {
                return View("Update", vm);
            } else {
                return View("Add", vm);
            }
        }
    }
}
