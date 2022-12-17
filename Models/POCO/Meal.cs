using ASP_PROJECT.DAL.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{
    public enum TypeService
    {
        Lunch,
        Evening
    }
    public abstract class Meal
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Type de service")]
        public TypeService Service { get; set; }

        [Required(ErrorMessage="Champs obligatoire")]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [DataType(DataType.Currency)]
        [Range(0.001, 1000000000, ErrorMessage = "Le prix doit être supérieur à 0")]
        [Display(Name = "Prix")]
        public double Price { get; set; }

        public Meal()
        {

        }

        public string ConvertTypeService()
        {
            string value = "";
            switch (this.Service)
            {
                case TypeService.Evening:
                    value = "Soir";
                    break;

                case TypeService.Lunch:
                    value = "Midi";
                    break;
            }
            return value;
        }
        // Ne fonctionne que si le menu ou le dish n'a pas été commandé ou si la dish ne se trouve pas dans un menu.
        public Meal Delete(IMenuDAL menuDAL) {
            try {
                return menuDAL.Delete(this);
            } catch (Exception e) {
                string message = e.Message;
                throw new Exception(message);
            }
        }

        public bool Update(IMenuDAL menuDAL) {
            try {
                if (this is Menu) {
                    return menuDAL.Update(this as Menu);
                }else {
                    return menuDAL.Update(this as Dish);
                }
            } catch (Exception e) {
                string message = e.Message;
                throw new Exception(message);
            }
        }

        public bool Add(IMenuDAL menuDAL,Restaurant restaurant) {
            try {
                if (this is Menu) {
                    return menuDAL.Add(this as Menu, restaurant);
                } else {
                    return menuDAL.Add(this as Dish, restaurant);
                } 
                } catch (Exception e) {
                string message = e.Message;
                throw new Exception(message);
            }
        }
    }
}