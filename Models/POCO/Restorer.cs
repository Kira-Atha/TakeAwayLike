using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.Models.POCO
{
    public class Restorer : Account
    {

        public List<Restaurant> RestaurantList { get; set; }

        public Restorer() : base()
        {
            RestaurantList = new List<Restaurant>();
        }

        public override bool Register(IAccountDAL accountDAL)
        {
            this.Password = Hash.CreateHash(this.Password);
            return accountDAL.SaveRestorer(this);
        }

        public static Restorer GetRestorerByMail(IAccountDAL accountDAL,string mail)
        {
            Restorer SearchedRestorer = accountDAL.GetRestorerByMail(mail);
            return SearchedRestorer;
        }

        public  bool ModifyRestorerInformations(IAccountDAL accountDAL)
        {
            return accountDAL.UpdateRestorerInformations(this);
        }

        public List<Restaurant> GetRestorerRestaurants(IRestaurantDAL restaurantDAL)
        {
            this.RestaurantList = restaurantDAL.GetRestorerRestaurantsById(this);
            return RestaurantList;
        }

        public static Restorer GetRestorerById(IAccountDAL accountDAL,int id)
        {
            Restorer restorer = new Restorer();
            restorer = accountDAL.GetRestorerById(id);
            return restorer;
        }


        public  bool SignRestaurant(Restaurant resto,IRestaurantDAL restaurantDAL)
        {
            try
            {
                return restaurantDAL.SignRestaurant(this, resto);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
