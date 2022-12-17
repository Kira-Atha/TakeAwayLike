using ASP_PROJECT.Models.POCO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels {
    public class MenuAndDishViewModel {

        public List<Dish> Dlist;
        public Menu Menu { get; set; }
        public int SelectedDish { get; set; }

        public List<int> SelectedList { get; set; }

        public Dish Dish { get; set; }
        public Restaurant Restaurant { get; set; }
        public bool DishOrMenu { get; set; }

        public MenuAndDishViewModel() {
            Menu = new Menu();
            Dish = new Dish();
            Dlist = new List<Dish>();
            SelectedList = new List<int>();
            Restaurant = new Restaurant();
        }

        public MenuAndDishViewModel(List<Dish> l) : this() {
            Dlist = l;
        }

        public List<SelectListItem> TypeService { get; set; } = new List<SelectListItem>{
            new SelectListItem(){Value="Lunch",Text="Diner"},
            new SelectListItem(){Value="Evening",Text="Soir"}
        };

        public List<SelectListItem> TypeDish { get; set; } = new List<SelectListItem>{
            new SelectListItem(){Value="Input",Text="Entrée"},
            new SelectListItem(){Value="Dish",Text="Plat"},
            new SelectListItem(){Value="Dessert",Text="Dessert"},
            new SelectListItem(){Value="Drink",Text="Boisson"},
            new SelectListItem(){Value="Accompaniment",Text="Accompagnement"}
        };
    }
}