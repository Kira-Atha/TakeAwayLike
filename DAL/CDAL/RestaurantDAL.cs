using ASP_PROJECT.DAL.IDAL;
using ASP_PROJECT.Models.POCO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_PROJECT.DAL.CDAL
{
    public class RestaurantDAL : IRestaurantDAL
    {
        private string connectionString;
        public RestaurantDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Restaurant GetRestaurantById(int restaurantId)
        {
            Restaurant r = new Restaurant();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT Name,City,Adress,PostalCode,PhoneNumber,RestaurantType,Description,TvaNumber,Country FROM dbo.Restaurants WHERE RestaurantId=@RestaurantId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestaurantId",restaurantId);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        r.Id = restaurantId;
                        r.Name = reader.GetString("Name");
                        r.City= reader.GetString("City");
                        r.Address= reader.GetString("Adress");
                        r.Pc = reader.GetString("PostalCode");
                        r.Tel= reader.GetString("PhoneNumber");
                        RestaurantType type;
                        Enum.TryParse(reader.GetString("RestaurantType"), out type);
                        r.Type = type;
                        r.Description= reader.GetString("Description");
                        r.NumTVA= reader.GetString("TvaNumber");
                        r.Country= reader.GetString("Country");
                    }
                }
            }
            return r;
        }

        public List<Restaurant> GetAllRestaurants() {
            List<Restaurant> restos = new List<Restaurant>();

            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string request = "SELECT * FROM dbo.Restaurants";
                SqlCommand cmd = new SqlCommand(request, connection);
                connection.Open();
                Restaurant resto = new Restaurant();
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                       
                        RestaurantType type;
                        Enum.TryParse(reader.GetString("RestaurantType"), out type);
                        resto.Type = type;
                        resto.Name = reader.GetString("Name");
                        resto.Address = reader.GetString("Adress");
                        resto.City = reader.GetString("City");
                        resto.Country = reader.GetString("Country");
                        resto.Description = reader.GetString("Description");
                        resto.Tel = reader.GetString("PhoneNumber");
                        resto.Pc = reader.GetString("PostalCode");
                        resto.NumTVA = reader.GetString("TvaNumber");
                        resto.Id = reader.GetInt32("RestaurantId");
                        restos.Add(resto);
                        resto = new Restaurant();
                    }
                }
            }
            foreach (var resto in restos)
            {
                GetRestaurantSchedules(resto);
            }
                return restos;
        }

        public List<Restaurant> GetRestorerRestaurantsById(Restorer restorer) {
            List<Restaurant> restos = new List<Restaurant>();


            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string request = "SELECT * FROM dbo.Restaurants WHERE RestorerId=@RestorerId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestorerId",restorer.Id);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        Restaurant resto = new Restaurant();
                        RestaurantType type;
                        Enum.TryParse(reader.GetString("RestaurantType"), out type);
                        resto.Type = type;
                        resto.Name = reader.GetString("Name");
                        resto.Address = reader.GetString("Adress");
                        resto.City = reader.GetString("City");
                        resto.Country = reader.GetString("Country");
                        resto.Description = reader.GetString("Description");
                        resto.Tel = reader.GetString("PhoneNumber");
                        resto.Pc = reader.GetString("PostalCode");
                        resto.NumTVA = reader.GetString("TvaNumber");
                        resto.Id = reader.GetInt32("RestaurantId");
                        restos.Add(resto);
                    }
                }
            }
            foreach (var resto in restos) {
                GetRestaurantSchedules(resto);
            }
            return restos;
        }
        public bool SignRestaurant(Restorer restorer, Restaurant restaurant)
        {
            bool success = false;
            //méthode pour vérifier que restaurant avec ce nom n'existe déjà pas avec ce nom 
            bool exists = VerifyExistingRestaurant(restaurant);
            if (exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string request = "INSERT INTO dbo.Restaurants (Name,City,Adress,PostalCode,PhoneNumber,RestaurantType,Description,TvaNumber,RestorerId,Country) VALUES (@Name,@City,@Adress,@PostalCode,@PhoneNumber,@RestaurantType,@Description,@TvaNumber,@RestorerId,@Country) ";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("RestorerId", restorer.Id);
                    cmd.Parameters.AddWithValue("Name", restaurant.Name);
                    cmd.Parameters.AddWithValue("City", restaurant.City);
                    cmd.Parameters.AddWithValue("Adress", restaurant.Address);
                    cmd.Parameters.AddWithValue("PostalCode", restaurant.Pc);
                    cmd.Parameters.AddWithValue("PhoneNumber", restaurant.Tel);
                    cmd.Parameters.AddWithValue("RestaurantType", restaurant.Type);
                    cmd.Parameters.AddWithValue("Description", restaurant.Description);
                    cmd.Parameters.AddWithValue("TvaNumber", restaurant.NumTVA);
                    cmd.Parameters.AddWithValue("Country", restaurant.Country);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }
                //get restaurant id 
                restaurant.Id = GetRestaurantId(restaurant);
                //save schedules in db en premier car ordre des FK
                success = SaveSchdeduleDay(restaurant);
            }
            else
            {
                throw new Exception("existing");
            }
            return success;
        }

        public int GetRestaurantId(Restaurant resto)
        {
            int id=0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT RestaurantId FROM dbo.Restaurants WHERE Name=@Name";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("Name", resto.Name);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("RestaurantId");
                    }
                }
            }
            return id;
        }

        public bool VerifyExistingRestaurant(Restaurant restaurant)
        {
            bool exists = false;
            List<string> Names = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT Name FROM dbo.Restaurants";
                SqlCommand cmd = new SqlCommand(request, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Names.Add(reader.GetString("Name"));
                    }
                }
            }
            foreach (var name in Names)
            {
                if (restaurant.Name == name)
                {
                    return true;
                }
            }
            return exists;
        }
        public bool SaveSchdeduleDay(Restaurant restaurant)
        {
            bool success = false;
            string day;
            DateTime opening;
            DateTime close;
            List<string> days = new List<string>()
            {
                "Lundi",
                "Mardi",
                "Mercredi",
                "Jeudi",
                "Vendredi",
                "Samedi",
                "Dimanche"
            };
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                for (int i = 0; i < 7; i++)
                {
                    day = days[i];
                    opening = restaurant.OpeningsTimes[i];
                    close = restaurant.CloseTimes[i];
                    string request = "INSERT INTO dbo.Schedules (RestaurantId,OpeningDay,OpenTime,CloseTime) VALUES (@RestaurantId,@OpeningDay,@OpenTime,@CloseTime) ";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("RestaurantId", restaurant.Id);
                    cmd.Parameters.AddWithValue("OpeningDay", day);
                    cmd.Parameters.AddWithValue("OpenTime", opening);
                    cmd.Parameters.AddWithValue("CloseTime", close);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                    connection.Close();
                }
            }
            return success;
        }
        public void GetRestaurantSchedules(Restaurant resto)
        {
            List<DateTime> OpeningTime = new List<DateTime>();
            List<DateTime> CloseTime = new List<DateTime>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT OpenTime,CloseTime FROM dbo.Schedules WHERE RestaurantId=@RestaurantId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestaurantId", resto.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OpeningTime.Add(reader.GetDateTime("OpenTime"));
                        CloseTime.Add(reader.GetDateTime("CloseTime"));
                    }
                }
                resto.OpeningsTimes = OpeningTime;
                resto.CloseTimes = CloseTime;
            }
        }
    }
}
