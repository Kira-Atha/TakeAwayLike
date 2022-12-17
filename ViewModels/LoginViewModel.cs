using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.ViewModels
{
    public class LoginViewModel
    {
        
        public Restorer user { get; set; }
        public LoginViewModel()
        {
            user = new Restorer();
        }

    }
}
