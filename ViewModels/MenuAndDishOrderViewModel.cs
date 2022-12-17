using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels {
    public class MenuAndDishOrderViewModel {
        
        public List<Dish> ListDishOrdered { get; set; }
        public List<Menu> ListMenuOrdered { get; set; }
        public Order Order { get; set; }

        public MenuAndDishOrderViewModel() {
            ListDishOrdered = new List<Dish>();
            ListMenuOrdered = new List<Menu>();
            Order = new Order();
        }
    }
}
