using ASP_PROJECT.DAL.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{
    public class Menu : Meal 
    {
        [Display(Name = "Liste des plats")]
        public List<Dish> DishList { get; set; }
        public Menu() : base() {
            DishList = new List<Dish>();
        }

        public Menu(List<Dish> dishes) : this()
        {
            DishList = dishes;
        }

        public static Menu GetMenuById(int MenuId, IMenuDAL menuDAL)
        {
            return menuDAL.GetMenuById(MenuId);
        }
    }
}
