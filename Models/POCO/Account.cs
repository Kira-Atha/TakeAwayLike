using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{ 
    public abstract class Account{

        public int Id { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(25, MinimumLength = 3)]
        [Display(Name = "Nom")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(25, MinimumLength = 3)]
        [Display(Name = "Prénom")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
   
        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "Le {0} doit avoir au moins {2} caractères de long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Pays")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(50, MinimumLength = 10)]
        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(35, MinimumLength = 5)]
        [Display(Name = "Ville")]
        public string City { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Téléphone")]
        [Phone]
        [RegularExpression(@"^(04([0-9]{2}[-. ]?){3}([0-9]{2}))|(071(([-. ]?[0-9]){2}){3})$", ErrorMessage = "Numéro de téléphone incorrect")]
        [DataType(DataType.PhoneNumber)]
        public string Tel { get; set; }

        
        [Display(Name = "Genre")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [StringLength(6, MinimumLength = 3)]
        [Display(Name = "Code Postal")]
        public string Pc { get; set; }

        public Account()
        {

        }
       
        public  Account Login(IAccountDAL accountDAL){
            try {
                return accountDAL.Login(this);
            } catch (Exception e) {
                string message = e.Message;
                throw new Exception(message);
            }
        }

        public bool VerifyExistingRestorer(IAccountDAL accountDAL)
        {
            return accountDAL.VerifyExistingRestorer((Restorer)this);
        }
       
        public bool VerifyExistingCustomer(IAccountDAL accountDAL)
        {
            return accountDAL.VerifyExistingCustomer((Customer)this);
        }

        public abstract bool Register(IAccountDAL accountDAL);
        
    }
}
