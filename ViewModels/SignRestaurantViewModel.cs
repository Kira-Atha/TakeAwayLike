using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels
{
    public class SignRestaurantViewModel
    {
        public int restorerId { get; set; }
        public Restaurant Resto { get; set; }
        //Days:  Opening Time and Close time
        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string MondayOt { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string MondayCt{get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string TuesdayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string TuesdayCt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string WednesdayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string WednesdayCt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string ThursdayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string ThursdayCt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string FridayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string FridayCt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string SaturdayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string SaturdayCt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string SundayOt { get;set;}

        [Required(ErrorMessage = "Champs obligatoire")]
        [RegularExpression(@"^(?:([01]?\d|2[0-3]):([0-5]?\d))?$", ErrorMessage = "Entrez les heures sous le format : Heures:Minutes")]
        public string SundayCt { get;set;}
        //public string DeliveryCity { get; set; }

        //public List<String> cities { get; set; }

        public SignRestaurantViewModel()
        {
            Resto = new Restaurant();
            //cities= new List<string>();
        }   
    }
}   
