using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.Other;
using ASP_PROJECT.Models.POCO;
using ASP_PROJECT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountDAL _accountDAL;

        public AccountController(IAccountDAL accountDAL)
        {
            _accountDAL = accountDAL;
        }
        public IActionResult Index() {
            return View();
        }

        public IActionResult Inscription()
        {
            return View("Inscription");
        }

        public IActionResult RestorerRegister()
        {
            Restorer r = new Restorer();
            return View("RestorerInscription", r);
        }
        public IActionResult CustomerInscription()
        {
            Customer customer = new Customer();
            return View("CustomerInscription",customer);
        }

        public IActionResult Login()
        {
            LoginViewModel vm = new LoginViewModel();
            return View("Login", vm);
        }

        public IActionResult ConsultRestorerInformations()
        {
            Restorer restorer = new Restorer();
            restorer.Firstname = HttpContext.Session.GetString("Firstname");
            restorer.Lastname = HttpContext.Session.GetString("Lastname");
            restorer.Id = (int)HttpContext.Session.GetInt32("restorerId");
            restorer.Email = HttpContext.Session.GetString("Email");
            restorer.City = HttpContext.Session.GetString("City");
            restorer.Address = HttpContext.Session.GetString("Address");
            restorer.Pc = HttpContext.Session.GetString("PostalCode");
            restorer.Tel = HttpContext.Session.GetString("PhoneNumber");
            restorer.Country = HttpContext.Session.GetString("Country");
            string gender= HttpContext.Session.GetString("Gender");
            restorer.Gender = gender[0];
            return View("ConsultRestorerInformations", restorer);
        }

        public IActionResult ConsultCustomerInformations()
        {
            Customer customer = new Customer();
            customer.Firstname = HttpContext.Session.GetString("Firstname");
            customer.Lastname = HttpContext.Session.GetString("Lastname");
            customer.Id = (int)HttpContext.Session.GetInt32("CustomerId");
            customer.Email = HttpContext.Session.GetString("Email");
            customer.City = HttpContext.Session.GetString("City");
            customer.Address = HttpContext.Session.GetString("Address");
            customer.Pc = HttpContext.Session.GetString("PostalCode");
            customer.Tel = HttpContext.Session.GetString("PhoneNumber");
            customer.Country = HttpContext.Session.GetString("Country");
            string dobStr= HttpContext.Session.GetString("DoB");
            customer.DoB = DateTime.Parse(dobStr);
            string gender = HttpContext.Session.GetString("Gender");
            customer.Gender = gender[0];
            return View("ConsultCustomerInformations", customer);
        }

        public void UpdateRestorerSessionInformations(Restorer Account)
        {
            Restorer BeforeModifications = Restorer.GetRestorerById(_accountDAL,Account.Id);
            Account.Country = BeforeModifications.Country;
            if (HttpContext.Session.GetString("restorerConnected")!="")
            {
                HttpContext.Session.SetString("restorerConnected", "true");
                HttpContext.Session.SetInt32("CustomerId", Account.Id);
                HttpContext.Session.SetString("Firstname", Account.Firstname);
                HttpContext.Session.SetString("Lastname", Account.Lastname);
                HttpContext.Session.SetString("Email", Account.Email);
                HttpContext.Session.SetString("City", Account.City);
                HttpContext.Session.SetString("Address", Account.Address);
                HttpContext.Session.SetString("PostalCode", Account.Pc);
                HttpContext.Session.SetString("PhoneNumber",Account.Tel);
                HttpContext.Session.SetString("Country",Account.Country);
                HttpContext.Session.SetString("Gender", Account.Gender.ToString());
            }
        }

        public void UpdateCustomerSessionInformations(Customer Account)
        {
            if (HttpContext.Session.GetString("customerConnected")!="")
            {
                HttpContext.Session.SetString("customerConnected", "true");
                HttpContext.Session.SetInt32("CustomerId", Account.Id);
                HttpContext.Session.SetString("Firstname", Account.Firstname);
                HttpContext.Session.SetString("Lastname", Account.Lastname);
                HttpContext.Session.SetString("Email", Account.Email);
                HttpContext.Session.SetString("City", Account.City);
                HttpContext.Session.SetString("Address", Account.Address);
                HttpContext.Session.SetString("PostalCode", Account.Pc);
                HttpContext.Session.SetString("PhoneNumber", Account.Tel);
                HttpContext.Session.SetString("Country", Account.Country);
                HttpContext.Session.SetString("DoB", ((Customer)Account).DoB.ToString("d"));
                HttpContext.Session.SetString("Gender", Account.Gender.ToString());
            }
        }

        public IActionResult ModifyRestorerInformations(string RestorerEmail)
        {
            Restorer RestorerToModify = Restorer.GetRestorerByMail(_accountDAL, RestorerEmail);
            return View("ModifyRestorerAccount",RestorerToModify);
        }
        public IActionResult ModifyCustomerInformations(string CustomerEmail)
        {
            
            Customer RestorerToModify = Customer.GetCustomerByMail(_accountDAL, CustomerEmail);
            return View("ModifyCustomerAccount", RestorerToModify);
        }

        public void SetCustomerSession(Account RecuperatedAccount)
        {
            HttpContext.Session.SetString("customerConnected", "true");
            HttpContext.Session.SetString("OrderExist", "true");
            HttpContext.Session.SetInt32("CustomerId", RecuperatedAccount.Id);
            HttpContext.Session.SetString("Firstname", RecuperatedAccount.Firstname);
            HttpContext.Session.SetString("Lastname", RecuperatedAccount.Lastname);
            HttpContext.Session.SetString("Email", RecuperatedAccount.Email);
            HttpContext.Session.SetString("Address", RecuperatedAccount.Address);
            HttpContext.Session.SetString("City", RecuperatedAccount.City);
            HttpContext.Session.SetString("PostalCode", RecuperatedAccount.Pc);
            HttpContext.Session.SetString("PhoneNumber", RecuperatedAccount.Tel);
            HttpContext.Session.SetString("Country", RecuperatedAccount.Country);
            HttpContext.Session.SetString("DoB", ((Customer)RecuperatedAccount).DoB.ToString("d"));
            HttpContext.Session.SetString("Gender", RecuperatedAccount.Gender.ToString());
            HttpContext.Session.SetString("OrderExist", "true");
            HttpContext.Session.SetString("DishesOrder", "");
            HttpContext.Session.SetString("MenusOrder", "");
            HttpContext.Session.SetString("currentRestaurantOrder", "");
        }

        public void SetRestorerSession(Account RecuperatedAccount)
        {
            HttpContext.Session.SetString("restorerConnected", "true");
            HttpContext.Session.SetString("OrderExist", "true");
            HttpContext.Session.SetInt32("restorerId", RecuperatedAccount.Id);
            HttpContext.Session.SetString("Firstname", RecuperatedAccount.Firstname);
            HttpContext.Session.SetString("Lastname", RecuperatedAccount.Lastname);
            HttpContext.Session.SetString("Email", RecuperatedAccount.Email);
            HttpContext.Session.SetString("City", RecuperatedAccount.City);
            HttpContext.Session.SetString("Address", RecuperatedAccount.Address);
            HttpContext.Session.SetString("PostalCode", RecuperatedAccount.Pc);
            HttpContext.Session.SetString("PhoneNumber", RecuperatedAccount.Tel);
            HttpContext.Session.SetString("Country", RecuperatedAccount.Country);
            HttpContext.Session.SetString("Gender", RecuperatedAccount.Gender.ToString());
            HttpContext.Session.SetString("currentRestaurantOrder", "");
        }

        public IActionResult Deconnexion()
        {
            HttpContext.Session.SetString("restorerConnected", "");
            HttpContext.Session.SetString("customerConnected", "");
            HttpContext.Session.SetString("Firstname", "");
            HttpContext.Session.SetString("Lastname", "");
            HttpContext.Session.SetString("Email", "");
            HttpContext.Session.SetString("City", "");
            HttpContext.Session.SetString("Address", "");
            HttpContext.Session.SetString("PostalCode", "");
            HttpContext.Session.SetString("PhoneNumber", "");
            HttpContext.Session.SetString("Country", "");
            HttpContext.Session.SetString("DoB", "");
            HttpContext.Session.SetString("Gender", "");
            HttpContext.Session.SetString("OrderExist", "");
            HttpContext.Session.SetString("currentRestaurantOrder","");
            TempData["Message"] = "";
            return RedirectToAction("Index", "Home");
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestorerRegister(Restorer r)
        {
            if (ModelState.IsValid)
            {
                r.Email = r.Email.ToLower();
                bool success = r.Register(_accountDAL);
                if (success == true)
                {
                    r = Restorer.GetRestorerByMail(_accountDAL, r.Email);
                    TempData["RegisterSuccess"] = "success";
                    SetRestorerSession((Account)r);
                    return RedirectToAction("ConsultRestorerRestaurants", "Restaurant", new { Id = r.Id });

                }
                else
                {
                    TempData["Error"] = "An account with this email adress already exists !";
                    return View("RestorerInscription", r);
                }
            }
            else
            {
                return View("RestorerInscription", r);
            }
        }

        
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerRegister(Customer accountC) {
            if (ModelState.IsValid) {
                accountC.Email = accountC.Email.ToLower();
                bool success = accountC.Register(_accountDAL);
                accountC = Customer.GetCustomerByMail(_accountDAL, accountC.Email);

                if (success == true) {
                    TempData["Message"] = "State0";
                    SetCustomerSession((Account)accountC);
                    return RedirectToAction("ConsultRestaurant", "Restaurant");
                } else {
                    TempData["Message"] = "State1";
                }
            } 
            return View("CustomerInscription",accountC);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            Account RecuperatedAccount;
            Account TryRestorer = new Restorer();
            Account TryCustomer = new Customer();
            bool IsRestorer = false;
            bool IsCustomer = false;
            TryRestorer.Email = vm.user.Email;
            TryRestorer.Password =vm.user.Password;

            TryCustomer.Email = vm.user.Email;
            TryCustomer.Password = vm.user.Password;
           

            if (vm.user.Email!=null && vm.user.Password!=null)
            {
                IsRestorer = TryRestorer.VerifyExistingRestorer(_accountDAL);
                if (IsRestorer == true)
                {
                    try
                    {
                        RecuperatedAccount = TryRestorer.Login(_accountDAL);
                        if (String.IsNullOrEmpty(HttpContext.Session.GetString("restorerConnected")))
                        {
                            TempData["Message"] = "State10";
                            SetRestorerSession(RecuperatedAccount);
                            return RedirectToAction("ConsultRestorerRestaurants", "Restaurant",new { Id = RecuperatedAccount.Id });
                        }
                        else
                        {
                            return View(); 
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["Exception"] = e.Message;
                        return View();

                    }
                }
                else
                {

                    IsCustomer = TryCustomer.VerifyExistingCustomer(_accountDAL);
                    if (IsCustomer == true)
                    {
                        try
                        {
                            RecuperatedAccount = TryCustomer.Login(_accountDAL);
                            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customerConnected")))
                            {
                                TempData["Message"] = "State11";
                                SetCustomerSession(RecuperatedAccount);
                                return RedirectToAction("ConsultRestaurant", "Restaurant");
                            }
                            else
                            {
                                return View();
                            }
                        }
                        catch(Exception e)
                        {
                            TempData["Exception"] = e.Message;
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Message"] = "State1";
                        return View();
                    }
                }
            }
            else
            {
                TempData["Message"] = "Uncompleted";
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyRestorerInformations(Restorer RestorerToModify)
        {
            string mail = HttpContext.Session.GetString("Email");
            Restorer RestOfInformations = Restorer.GetRestorerByMail(_accountDAL,mail);
            RestorerToModify.Id = RestOfInformations.Id;
            RestorerToModify.Email = RestOfInformations.Email;
            if (RestorerToModify.Firstname != null
                && RestorerToModify.Lastname != null
                && RestorerToModify.Gender != 0
                && RestorerToModify.Address != null
                && RestorerToModify.City != null
                && RestorerToModify.Address != null
                && RestorerToModify.Pc != null
                && RestorerToModify.Tel != null)
            {
                bool success = RestorerToModify.ModifyRestorerInformations(_accountDAL);
                if (success == true)
                {
                    UpdateRestorerSessionInformations(RestorerToModify);
                    TempData["AccountModifications"] = "success";
                    return RedirectToAction("ConsultRestorerInformations");
                }
            }
            else
            {
                TempData["AccountModifications"] = "invalid";
            }
            return View("ModifyRestorerAccount", RestorerToModify);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyCustomerInformations(Customer CustomerToModify)
        {
            string mail = HttpContext.Session.GetString("Email");
            Customer RestOfInformations = Customer.GetCustomerByMail(_accountDAL, mail);
            CustomerToModify.Id = RestOfInformations.Id;
            CustomerToModify.Email = RestOfInformations.Email;
            CustomerToModify.Country = RestOfInformations.Country;
            if (CustomerToModify.Firstname != null
                && CustomerToModify.Lastname != null
                && CustomerToModify.Gender != 0
                && CustomerToModify.Address != null
                && CustomerToModify.City != null
                && CustomerToModify.Address != null
                && CustomerToModify.Pc != null
                && CustomerToModify.Tel != null)
            {
                bool success = CustomerToModify.ModifyCustomerInformations(_accountDAL);
                if (success == true)
                {
                    UpdateCustomerSessionInformations(CustomerToModify);
                    TempData["AccountModifications"] = "success";
                    return RedirectToAction("ConsultCustomerInformations");

                }
            }
            else
            {
                TempData["AccountModifications"] = "invalid";
            }
            return View("ModifyCustomerAccount", CustomerToModify);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Inscription(string type)
        {
            if (type == "restorer")
            {
                return RedirectToAction("RestorerRegister");
            }
            else
            { 
                return RedirectToAction("CustomerInscription");
            }
        }
    }
}
