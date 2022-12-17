using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels
{
    public class CustomerOrderViewModel
    {
        public List<Order> CustomerOrders { get; set; }
        public Customer Customer { get; set; }

        public Restaurant Restaurant { get; set; }

        public int OrderStatus;

        public CustomerOrderViewModel()
        {
            CustomerOrders = new List<Order>();
            Customer = new Customer();
            Restaurant = new Restaurant();
        }
        public CustomerOrderViewModel(List<Order> OrdersDb) : this()
        {
            CustomerOrders = OrdersDb;
        }
    }
}
