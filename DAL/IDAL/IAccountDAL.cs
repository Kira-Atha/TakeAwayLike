using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.DAL.IDAL
{
    public interface IAccountDAL
    {
        bool SaveRestorer(Restorer r);

        bool SaveCustomer(Customer accountC);

        Account Login(Account account);

        public bool VerifyExistingRestorer(Restorer r);
        public bool VerifyExistingCustomer(Customer c);

        public Restorer GetRestorerByMail(string accountMail);  
        public Customer GetCustomerByMail(string accountMail);

        public bool UpdateRestorerInformations(Restorer restorerToModify);

        public bool UpdateCustomerInformations(Customer customerToModify);

        public Customer GetCustomerById(int customerId);
        public Restorer GetRestorerById(int restorerId);
    }
}
