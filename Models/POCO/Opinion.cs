using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{ 
    public class Opinion
    {

        [Display(Name = "Commentaire")]
        [StringLength(500)]
        public string OpinionDescription { get; set; }

        [Display(Name = "Note")]
        [Required(ErrorMessage = "Une note est obligatoire")]
        public int Note { get; set; }

        public Opinion()
        {

        }
        public void ModerateOpinion()
        {

        }
    }
}
