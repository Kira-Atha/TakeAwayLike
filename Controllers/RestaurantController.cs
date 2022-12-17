using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.POCO;
using ASP_PROJECT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Controllers {
    public class RestaurantController : Controller {
        

        private readonly IRestaurantDAL _restaurantDAL;
        private readonly IMenuDAL _menuDAL;
        private readonly IAccountDAL _accountDAL;


        public RestaurantController(IRestaurantDAL restaurantDAL,IMenuDAL menuDAL, IAccountDAL accountDAL) {
            _restaurantDAL = restaurantDAL;
            _menuDAL = menuDAL;
            _accountDAL = accountDAL;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult ConsultRestaurant() {
            List<Restaurant> restos = Restaurant.GetAllRestaurants(_restaurantDAL);
            ListRestaurantsViewModel viewModel = new ListRestaurantsViewModel(restos);
            TempData["ItemAdded"] = "";
            return View("Views/Restaurant/ConsultAllRestaurants.cshtml", viewModel);
        }

        public IActionResult SignRestaurant(int restorerId)
        {
            TempData["RestaurantSignError"] = null;
            TempData["otBiggerCt"] = null;
            SignRestaurantViewModel vm = new SignRestaurantViewModel();
            vm.restorerId = restorerId;
            return View("SignRestaurant", vm);
        }

        public IActionResult ConsultRestorerRestaurants()
        {
            TempData["ItemAdded"] = "";
            Restorer r = new Restorer();
            int id = (int)HttpContext.Session.GetInt32("restorerId");
            r = Restorer.GetRestorerById(_accountDAL,id);
            r.RestaurantList=r.GetRestorerRestaurants(_restaurantDAL);
            ListRestaurantsViewModel viewModel = new ListRestaurantsViewModel(r.RestaurantList);
            viewModel.Restorer = r;
            return View("ConsultRestorerRestaurants", viewModel);
        }
        public IActionResult RestorerAcceuil()
        {
            Restorer restorer = new Restorer();
            restorer.Id =(int)HttpContext.Session.GetInt32("restorerId");
            return RedirectToAction("ConsultRestorerRestaurants", restorer);
        }

        public IActionResult ConsultAll(int restaurantId)
        {
            Restaurant resto = new Restaurant();
            resto.Id = restaurantId;
            HttpContext.Session.SetInt32("restaurantId", restaurantId);
            resto = resto.GetRestaurantDishesAndMenus(_restaurantDAL, _menuDAL);
            resto = resto.GetScheduleResto(_restaurantDAL);
            ListRestaurantsViewModel vm = new ListRestaurantsViewModel(resto);
            return View("Views/Restaurant/ConsultRestaurantMenuDish.cshtml", vm);
        }

        public bool VerifySignRestaurant(SignRestaurantViewModel vm)
        {
            //ct -> closetime
            //ot -> opentime
            int errors = 0;
            int ErrorOtBiggerThanCt = 0;
            bool success = false;
            errors += vm.MondayCt == null ? 1 : 0 ;
            errors += vm.MondayCt == null ? 1 : 0;
            errors += vm.TuesdayCt == null ? 1 : 0;
            errors += vm.WednesdayCt == null ? 1 : 0;
            errors += vm.FridayCt == null ? 1 : 0;
            errors += vm.SaturdayCt == null ? 1 : 0;
            errors += vm.SaturdayCt == null ? 1 : 0;
            errors += vm.MondayOt == null ? 1 : 0;
            errors += vm.TuesdayOt == null ? 1 : 0;
            errors += vm.WednesdayOt == null ? 1 : 0;
            errors += vm.FridayOt == null ? 1 : 0;
            errors += vm.SaturdayOt == null ? 1 : 0;
            errors += vm.SaturdayOt == null ? 1 : 0;

            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.MondayOt) > Convert.ToDateTime(vm.MondayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.TuesdayOt) > Convert.ToDateTime(vm.TuesdayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.WednesdayOt) > Convert.ToDateTime(vm.WednesdayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.ThursdayOt) > Convert.ToDateTime(vm.ThursdayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.FridayOt) > Convert.ToDateTime(vm.FridayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.SundayOt) > Convert.ToDateTime(vm.SundayCt) ? 1 : 0;
            ErrorOtBiggerThanCt += Convert.ToDateTime(vm.SaturdayOt) > Convert.ToDateTime(vm.SaturdayCt) ? 1 : 0;
            if (ErrorOtBiggerThanCt > 0)
            {
                TempData["otBiggerCt"] = "true";
            }
            else
            {
                TempData["otBiggerCt"] = "false";
            }
            errors += vm.Resto.Name == "" ? 1 : 0;
            errors += vm.Resto.Description == "" ? 1 : 0;
            errors += vm.Resto.Address == "" ? 1 : 0;
            errors += vm.Resto.City == "" ? 1 : 0;
            errors += vm.Resto.Pc == "" ? 1 : 0;
            errors += vm.Resto.Country == "" ? 1 : 0;
            errors += vm.Resto.Tel == "" ? 1 : 0;
            errors += vm.Resto.NumTVA == "" ? 1 : 0;

            if (errors > 0)
            {
                success = false;
            }
            else
            {
                success = true;
            }
            return success;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignRestaurant(SignRestaurantViewModel vm)
        {
            bool valid = VerifySignRestaurant(vm);
            vm.restorerId = (int)HttpContext.Session.GetInt32("restorerId");
            if (valid)
            {
                Restaurant CreatedRestaurant = vm.Resto;
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.MondayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.TuesdayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.WednesdayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.ThursdayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.FridayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.SaturdayOt));
                CreatedRestaurant.OpeningsTimes.Add(DateTime.Parse(vm.SundayOt));

                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.MondayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.TuesdayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.WednesdayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.ThursdayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.FridayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.SaturdayCt));
                CreatedRestaurant.CloseTimes.Add(DateTime.Parse(vm.SundayCt));

                try
                {
                    Restorer restorer = new Restorer();
                    restorer = Restorer.GetRestorerById(_accountDAL,vm.restorerId);
                    HttpContext.Session.SetInt32("restoredId", restorer.Id);
                    bool success = restorer.SignRestaurant(CreatedRestaurant, _restaurantDAL);
                    if (success == true)
                    {
                        TempData["RestaurantSign"] = "success";
                        return RedirectToAction("ConsultRestorerRestaurants");
                    }
                    else
                    {
                        TempData["RestaurantSignError"] = "error";
                        return View("SignRestaurant", vm);

                    }
                }catch (Exception e)
                {
                    TempData["RestaurantSignError"] = e.Message;
                    return View("SignRestaurant", vm);
                }
            }
            else
            {
                return View("SignRestaurant", vm);
            }
        }
    }
}