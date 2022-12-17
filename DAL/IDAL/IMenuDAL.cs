
using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.DAL.IDAL
{
    public interface IMenuDAL
    {

        List<Dish> GetDishes(Restaurant restaurant);
        List<Menu> GetMenus(Restaurant resto);
        Dish GetDishById(int id);
        Menu GetMenuById(int MenuId);
        Meal Delete(Meal meal);
        bool Update(Meal meal);
        bool Add(Meal meal,Restaurant restaurant);

    }
}
