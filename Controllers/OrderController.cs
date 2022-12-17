using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.POCO;
using ASP_PROJECT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMenuDAL _menuDAL;
        private readonly IOrderDAL _orderDAL;
        private readonly IAccountDAL _accountDAL;
        private readonly IRestaurantDAL _restaurantDAL;

        public OrderController(IMenuDAL menuDAL, IOrderDAL orderDAL,IAccountDAL accountDAL,IRestaurantDAL restaurantDAL)
        {
            _menuDAL = menuDAL;
            _orderDAL = orderDAL;
            _accountDAL = accountDAL;
            _restaurantDAL = restaurantDAL;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConsultRestaurantOrders(int restoId)
        {
            RestaurantOrderViewModel vm = new RestaurantOrderViewModel();
            Restaurant resto = new Restaurant();
            resto.Id = restoId;
            HttpContext.Session.SetInt32("restaurantId", restoId);
            List<Order> RestaurantOrders = resto.GetRestaurantOrders(_orderDAL,_menuDAL,_accountDAL);
            // Passer par une liste intermédiaire pour faire des tests et ensuite compléter la OrdersList de l'objet
            if (RestaurantOrders.Count == 0)
            {
                TempData["listOrders"] = "empty";
            }
            else
            {
                TempData["listOrders"] = "";

            }
            vm.Restaurant.OrdersList = RestaurantOrders;
            return View("ConsultRestaurantOrders",vm);
        }
        public IActionResult ConsultCustomerOrders()
        {
            CustomerOrderViewModel vm = new CustomerOrderViewModel();
            Customer customer = new Customer();
            customer.Id = (int)HttpContext.Session.GetInt32("CustomerId");
            customer.OrdersList = customer.GetCustomerOrders(_orderDAL, _menuDAL, _restaurantDAL);
            vm.Customer.OrdersList = customer.OrdersList;
            if (customer.OrdersList.Count == 0)
            {
                TempData["listOrders"] = "empty";
            }
            else
            {
                TempData["listOrders"] = "";

            }
            return View("ConsultCustomerOrders",vm);
        }

        [HttpPost]
        public IActionResult ModifyOrderStatus(int orderId)
        {
            Order searchedOrder = Order.GetOrderById(_orderDAL, orderId);
            HttpContext.Session.SetInt32("orderIdToModify",searchedOrder.Id);
            return View("UpdateOrderStatus",searchedOrder);
        }

        public IActionResult AddMenuToCart(int menuId)
        {
            TempData["ItemAdded"] = "";
            int restoId = (int)HttpContext.Session.GetInt32("restaurantId");
            string var = HttpContext.Session.GetString("currentRestaurantOrder");

            if (HttpContext.Session.GetString("currentRestaurantOrder") == "")
            {
                HttpContext.Session.SetString("currentRestaurantOrder", restoId.ToString());
                if (HttpContext.Session.GetString("OrderExist") == "true")
                {
                    if (HttpContext.Session.GetString("MenusOrder") != "")
                    {
                        string sessionMenusIds = HttpContext.Session.GetString("MenusOrder");
                        sessionMenusIds += ";" + menuId.ToString();
                        HttpContext.Session.SetString("MenusOrder", sessionMenusIds);
                        TempData["ItemAdded"] = "menu";
                    }
                    else
                    {
                        HttpContext.Session.SetString("MenusOrder", menuId.ToString());
                        TempData["ItemAdded"] = "menu";
                    }
                }
            }
            else
            {
                string currentRestaurantOrder = HttpContext.Session.GetString("currentRestaurantOrder");
                if (restoId.ToString() !=  currentRestaurantOrder)
                {
                    TempData["ErreurAjout"] = "Vous ne pouvez pas commander dans 2 restaurant à la fois ! ";
                    return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = restoId });
                }
                else
                {
                    if (HttpContext.Session.GetString("OrderExist") == "true")
                    {
                        if (HttpContext.Session.GetString("MenusOrder") != "")
                        {
                            string sessionMenusIds = HttpContext.Session.GetString("MenusOrder");
                            sessionMenusIds += ";" + menuId.ToString();
                            HttpContext.Session.SetString("MenusOrder", sessionMenusIds);
                            TempData["ItemAdded"] = "menu";
                        }
                        else
                        {
                            HttpContext.Session.SetString("MenusOrder", menuId.ToString());
                            TempData["ItemAdded"] = "menu";
                        }
                    }
                }
            }
            return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = restoId });
        }

        public IActionResult AddDishToCart(int dishId)
        {
            TempData["ItemAdded"] = "";
            if (HttpContext.Session.GetString("OrderExist") == "true")
            {
                if (HttpContext.Session.GetString("DishesOrder") != "")
                {
                    string sessionDishesIds = HttpContext.Session.GetString("DishesOrder");
                    sessionDishesIds += ";" + dishId.ToString();
                    HttpContext.Session.SetString("DishesOrder", sessionDishesIds);
                    TempData["ItemAdded"] = "plat";
                }
                else
                {
                    HttpContext.Session.SetString("DishesOrder", dishId.ToString());
                    TempData["ItemAdded"] = "plat";
                }
            }
            int restoId = (int)HttpContext.Session.GetInt32("restaurantId");
            
            return RedirectToAction("ConsultAll", "Restaurant", new { restaurantId = restoId });
        }

        public IActionResult ConsultCart()
        {
            Order order = new Order();

            MenuAndDishOrderViewModel vm = new MenuAndDishOrderViewModel();

            if (HttpContext.Session.GetString("customerConnected") == "true")
            {

                if (HttpContext.Session.GetString("DishesOrder") != "" || HttpContext.Session.GetString("MenusOrder") != "") { 
                    vm.Order= GetOrdersInformations(order);
                    foreach (var meal in order.ListMealOrdered) {
                        if (meal is Menu) {
                            vm.ListMenuOrdered.Add(meal as Menu);
                        } else {
                            vm.ListDishOrdered.Add(meal as Dish);
                        }
                    }
                }
                else
                {
                    TempData["MessageCart"] = "vide";
                    return View("Views/Order/ConsultCart.cshtml", vm);
                }
            }
            return View("Views/Order/ConsultCart.cshtml", vm);
        }

        public IActionResult DeleteDishOrdered(int dishId)
        {
            string dishesSession = HttpContext.Session.GetString("DishesOrder");
            string[] sessionDishesId = dishesSession.Split(";");
            string newDishSession = "";
            string receivedId = dishId.ToString();
            int flag = 0;
            foreach (var id in sessionDishesId)
            {
                if (id != receivedId)
                {
                    if (newDishSession != "")
                    {
                        newDishSession += ";" + id;
                    }
                    else
                    {
                        newDishSession += id;
                    }
                }
                //gestion doublon : supprimer un seul exemplaire 
                if (receivedId == id && flag == 1)
                {
                    if (newDishSession != "")
                    {
                        newDishSession += ";" + id;
                    }
                    if (newDishSession == "")
                    {
                        newDishSession += id;
                    }
                    flag = 2;
                }
                //Permet de gérer le cas ou on voudrait supprimer un doublon d'un dish déjà ajouté
                if (receivedId == id && flag == 0)
                {
                    flag = 1;
                }
            }
            HttpContext.Session.SetString("DishesOrder",newDishSession);
            return RedirectToAction("ConsultCart");
        }
        public IActionResult DeleteMenuOrdered(int menuId)
        {
            string menusSession = HttpContext.Session.GetString("MenusOrder");
            string[] sessionMenusId = menusSession.Split(";");
            string newMenuSession = "";
            string receivedId = menuId.ToString();
            int flag = 0;
            foreach (var id in sessionMenusId)
            {
                if (id != receivedId)
                {
                    if (newMenuSession != "")
                    {
                        newMenuSession += ";" + id;
                    }
                    else
                    {
                        newMenuSession += id;
                    }
                }
                //gestion doublon : supprimer un seul exemplaire 
                if (receivedId == id && flag == 1)
                {
                    if (newMenuSession != "")
                    {
                        newMenuSession += ";" + id;
                    }
                    if (newMenuSession == "")
                    {
                        newMenuSession += id;
                    }
                    flag = 2;
                }
                //Permet de gerer le cas ou on voudrait supprimer un doublon d'un dish déjà ajouté
                if (receivedId == id && flag == 0)
                {
                    flag = 1;
                }
            }
            HttpContext.Session.SetString("MenusOrder", newMenuSession);
            return RedirectToAction("ConsultCart");
        }

        public IActionResult DeleteCart()
        {
            EmptyCart();
            TempData["MessageCart"] = "vide";
            MenuAndDishOrderViewModel vm = new MenuAndDishOrderViewModel();

            return View("Views/Order/ConsultCart.cshtml",vm);
        }

        public void EmptyCart()
        {
            HttpContext.Session.SetString("MenusOrder", "");
            HttpContext.Session.SetString("DishesOrder", "");
        }

        public Order GetOrdersInformations(Order order)
        {
            string sessionMenusIds = HttpContext.Session.GetString("MenusOrder");
            if (sessionMenusIds != "")
            {
                string[] menusIdsSplited = sessionMenusIds.Split(";");
                foreach (var item in menusIdsSplited)
                {
                    int menuId = Int32.Parse(item);
                    Menu menuAdded = Menu.GetMenuById(menuId, _menuDAL);
                    order.ListMealOrdered.Add(menuAdded);
                }
            }


            string sessionDishesIds = HttpContext.Session.GetString("DishesOrder");

            if (sessionDishesIds != "")
            {
                string[] dishesIdsSplited = sessionDishesIds.Split(";");
                foreach (var dId in dishesIdsSplited)
                {
                    int dishId = Int32.Parse(dId);
                    Dish dishAdded = Dish.GetDishById(dishId, _menuDAL);
                    order.ListMealOrdered.Add(dishAdded);
                }
            }

            int customerId = (int)HttpContext.Session.GetInt32("CustomerId");
            Restaurant restaurant = new Restaurant();
            int Id = (int)HttpContext.Session.GetInt32("restaurantId");

            restaurant = Restaurant.GetRestaurantById(_restaurantDAL, Id);
            Customer customer = Customer.GetCustomerById(_accountDAL, customerId);

            order.Customer = customer;
            order.Restaurant = restaurant;
            order.CalculateTotalPrice();
            return order;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateOrder(Order order)
        {
            MenuAndDishOrderViewModel vm = new MenuAndDishOrderViewModel();

            order = GetOrdersInformations(order);
            order.DeliveryAdress = order.Customer.Address + "," + order.Customer.City + "," + order.Customer.Pc;
            order.DateOrder = DateTime.Now;
            order.Status = 0;
            Customer customer = order.Customer;
            bool success = order.ValidateOrder(_orderDAL);
            EmptyCart();
            TempData["MessageCart"] = "vide";
            TempData["StatutOrder"] = "OK";
            TempData["StatutOrder"] = "NOTOK";
            vm.Order = order;
            return View("ResumeOrder", vm);




        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateUpdatingOrderStatus(Order order)
        {
            int orderId = (int)HttpContext.Session.GetInt32("orderIdToModify");
            order.Id = orderId;
            bool success = order.UpdateOrderStatus(_orderDAL);
            if (success == true)
            {
                HttpContext.Session.SetInt32("orderIdToModify", 0);
                int restaurantId = (int)HttpContext.Session.GetInt32("restaurantId");
                return RedirectToAction("ConsultRestaurantOrders", new { restoId = restaurantId });
            }
            else
            {
                return View("UpdateOrderStatus", order);
            }
        }
    }
}
