using ASP_PROJECT.DAL.IDAL;
using System;
using System.Collections.Generic;
using ASP_PROJECT.Models.Other;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{
    public class Customer : Account
    {
        [Display(Name = "Date de naissance")]
        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }

        public List<Order> OrdersList { get; set; }
        public List<Opinion> OpinionList { get; set; }

        public Customer() : base(){
            OrdersList=new List<Order>();
            OpinionList = new List<Opinion>();
        }

        public Customer(List<Order> Orders,List<Opinion> Opinions) : this()
        {
            OrdersList = Orders;
            OpinionList = Opinions;
        }
        //public void GiveAnOpinion()
        //{

        //}
        public override bool Register(IAccountDAL accountDAL) {
            this.Password = Hash.CreateHash(this.Password);
            return accountDAL.SaveCustomer(this);
        }

        public static Customer GetCustomerByMail(IAccountDAL accountDAL, string mail)
        {
            Customer SearchedCustomer = accountDAL.GetCustomerByMail(mail);
            return SearchedCustomer;
        }

        public static Customer GetCustomerById(IAccountDAL accountDAL, int customerId)
        {
            Customer SearchedCustomer = accountDAL.GetCustomerById(customerId);
            return SearchedCustomer;
        }

        public  bool ModifyCustomerInformations(IAccountDAL accountDAL)
        {
            return accountDAL.UpdateCustomerInformations(this);
        }

        public  List<Order> GetCustomerOrders( IOrderDAL orderDAL, IMenuDAL menuDAL, IRestaurantDAL restaurantDAL)
        {
            OrdersList = orderDAL.GetCustomerOrders(this);
            foreach (var order in OrdersList)
            {
                List<int> MenuDetailsId = orderDAL.GetMenusIdInMenuDetails(order);
                List<int> DishDetailsId = orderDAL.GetDishesIdInMenuDetails(order);
                List<Meal> OrderMeals = new List<Meal>();
                Restaurant restaurant = restaurantDAL.GetRestaurantById(order.Restaurant.Id);
                order.Restaurant = restaurant;
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
                    if (meal is Menu){
                        order.ListMealOrdered.Add(meal as Menu);
                    }else{
                        order.ListMealOrdered.Add(meal as Dish);
                    }
                }
            }
            return OrdersList;
        }
    }
}
