using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels
{
    public class RestaurantOrderViewModel
    {
        public List<Order> RestaurantOrders { get; set; }
        public Restaurant Restaurant { get; set; }

        public int OrderStatus;

        public RestaurantOrderViewModel()
        {
            RestaurantOrders = new List<Order>();
            Restaurant = new Restaurant();
        }
        public RestaurantOrderViewModel(List<Order> OrdersDb) :this()
        {
            RestaurantOrders = OrdersDb;
        }
    }
}
