using ASP_PROJECT.DAL.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{
    public enum RestaurantType
    {
        Asian,
        Mexican,
        Snack,
        African,
        Thaï,
        Greek,
        Sushi,
        Spanish,
        Pizza,
        Italian
    }
   
    public class Restaurant
    {
        public int Id { get; set; }
        public RestaurantType Type { get; set; }

        [Display(Name="Nom du restaurant")]
        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(50, MinimumLength = 10)]
        [Display(Name = "Adresse du restaurant")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(35, MinimumLength = 5)]
        [Display(Name = "Ville du restaurant")]
        public string City { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Pays du restaurant")]
        public string Country { get; set; }

        [StringLength(500)]
        [Display(Name="Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Téléphone du restaurant")]
        [DataType(DataType.PhoneNumber)]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(6, MinimumLength = 3)]
        [Display(Name = "Code Postal")]
        public string Pc { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^\s*$|(ATU[0-9]{8}|BE[01][0-9]{9}|BG[0-9]{9,10}|HR[0-9]{11}|CY[A-Z0-9]{9}|CZ[0-9]{8,10}|DK[0-9]{8}|EE[0-9]{9}|FI[0-9]{8}|FR[0-9A-Z]{2}[0-9]{9}|DE[0-9]{9}|EL[0-9]{9}|HU[0-9]{8}|IE([0-9]{7}[A-Z]{1,2}|[0-9][A-Z][0-9]{5}[A-Z])|IT[0-9]{11}|LV[0-9]{11}|LT([0-9]{9}|[0-9]{12})|LU[0-9]{8}|MT[0-9]{8}|NL[0-9]{9}B[0-9]{2}|PL[0-9]{10}|PT[0-9]{9}|RO[0-9]{2,10}|SK[0-9]{10}|SI[0-9]{8}|ES[A-Z]([0-9]{8}|[0-9]{7}[A-Z])|SE[0-9]{12}|GB([0-9]{9}|[0-9]{12}|GD[0-4][0-9]{2}|HA[5-9][0-9]{2}))$", ErrorMessage = "Numéro de TVA Incorrect")]
        [Display(Name = "N° TVA")]
        public string NumTVA { get; set; }

        public List<string> DeliveryCities { get; set; }
        public List<DateTime> OpeningsTimes { get; set; }
        public List<DateTime> CloseTimes { get; set; }

        public List<Opinion> OpinionList { get; set; }
        public List<Meal> MealList { get; set; }
        public List<Order> OrdersList { get; set; }

        public Restaurant()
        {
            OpeningsTimes = new List<DateTime>();
            CloseTimes = new List<DateTime>();
            DeliveryCities = new List<string>();
            MealList = new List<Meal>();
            OpinionList = new List<Opinion>();
            OrdersList = new List<Order>();
        }
        public Restaurant(List<Meal> meallist, List<Order> orderslist, List<Opinion> opinionlist):this()
        {
            MealList = meallist;
            OrdersList = orderslist;
            OpinionList = opinionlist;
        }
        public Restaurant(string name,string adress,string country,string description,string tel,string pc,string ntva,string type):this()
        {
            Name = name;
            Address = adress;
            Country = country;
            Description = description;
            Tel = tel;
            Pc = pc;
            NumTVA = ntva;
            RestaurantType t;
            Enum.TryParse(type, out t);
            Type = t;
        }

        public string ConvertRestaurantType()
        {
            string value = "";
            switch (this.Type)
            {
                case RestaurantType.Asian:
                    value = "Asiatique";
                    break;

                case RestaurantType.Mexican:
                    value = "Mexicain";
                    break;

                case RestaurantType.Snack:
                    value = "Snack";
                    break;

                case RestaurantType.African:
                    value = "Africain";
                    break;

                case RestaurantType.Thaï:
                    value = "Thaî";
                    break;

                case RestaurantType.Greek:
                    value = "Grec";
                    break;

                case RestaurantType.Sushi:
                    value = "Sushi";
                    break;

                case RestaurantType.Spanish:
                    value = "Espagnol";
                    break;

                case RestaurantType.Pizza:
                    value = "Pizza";
                    break;

                case RestaurantType.Italian:
                    value = "Italien";
                    break;
            }
            return value;

        }

        public static List<Restaurant> GetAllRestaurants(IRestaurantDAL DAL) {
            return DAL.GetAllRestaurants();
        }

        public Restaurant GetRestaurantDishesAndMenus(IRestaurantDAL restaurantDAL,IMenuDAL menuDAL)
        {
            Restaurant recuperatedResto = restaurantDAL.GetRestaurantById(this.Id);
            List<Menu> restaurantMenus = menuDAL.GetMenus(this);
            foreach (var menu in restaurantMenus) {
                recuperatedResto.MealList.Add(menu);
            }
            List<Dish> restaurantDishes = menuDAL.GetDishes(this);
            foreach (var dish in restaurantDishes) {
                recuperatedResto.MealList.Add(dish);
            }
            return recuperatedResto;
        }

        public Restaurant GetScheduleResto(IRestaurantDAL restoDAL)
        {
            restoDAL.GetRestaurantSchedules(this);
            return this;
        }

        public static Restaurant GetRestaurantById(IRestaurantDAL restoDAL,int id)
        {
            return restoDAL.GetRestaurantById(id);
        }

        public List<Dish> GetDishes(IMenuDAL menuDAL)
        {
            return menuDAL.GetDishes(this);
        }

        public List<Menu> GetMenus(IMenuDAL menuDAL)
        {
            return menuDAL.GetMenus(this);
        }
        // VALIDATE ORDER !!!!!

        public List<Order> GetRestaurantOrders(IOrderDAL orderDAL, IMenuDAL menuDAL, IAccountDAL accountDAL)
        {
            this.OrdersList = orderDAL.GetRestaurantOrders(this);
            foreach (var order in OrdersList)
            {
                List<int> MenuDetailsId = orderDAL.GetMenusIdInMenuDetails(order);
                List<int> DishDetailsId = orderDAL.GetDishesIdInMenuDetails(order);
                List<Meal> OrderMeals = new List<Meal>();
                Customer customer = accountDAL.GetCustomerById(order.Customer.Id);
                order.Customer = customer;
                foreach (var menuId in MenuDetailsId)
                {
                    OrderMeals.Add(menuDAL.GetMenuById(menuId));
                }
                foreach (var dishId in DishDetailsId)
                {
                    OrderMeals.Add(menuDAL.GetDishById(dishId));
                }
                foreach (var meal in OrderMeals)
                {
                    if (meal is Menu)
                    {
                        order.ListMealOrdered.Add(meal as Menu);
                    }
                    if (meal is Dish)
                    {
                        order.ListMealOrdered.Add(meal as Dish);
                    }
                }
            }
            return OrdersList;
        }
    }
}
