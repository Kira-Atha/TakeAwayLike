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
    public class MenuDAL : IMenuDAL
    {
        private string connectionString;
        public MenuDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int GetMenuIdByName(Menu menu)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT MenuId FROM dbo.Menus WHERE Name=@Name";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("Name", menu.Name);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("MenuId");
                    }
                }
            }
            return id;
        }

        public bool AddingMenuDetails(Menu menu)
        {
            bool success = false;
            string request = "INSERT INTO dbo.MenuDetails(DishId,MenuId) VALUES (@DishId,@MenuId)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                foreach (var dish in menu.DishList)
                {
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("DishId", dish.Id);
                    cmd.Parameters.AddWithValue("MenuId", menu.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                    connection.Close();
                }
            }
            return success;
        }
        public List<Dish> GetDishes(Restaurant r)
        {
            List<Dish> listDishes = new List<Dish>();
            Dish temp = new Dish();
            TypeService serviceType;
            TypeDish typeDish;
            decimal price;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT DishId,Name,Price,TypeService,Description,TypeDish FROM dbo.Dishes WHERE RestaurantId=@RestaurantId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestaurantId", r.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        temp.Id = reader.GetInt32("DishId");
                        temp.Name=reader.GetString("Name");
                        price=reader.GetDecimal("Price");
                        temp.Price = (double)price;
                        Enum.TryParse(reader.GetString("TypeService"), out serviceType);
                        temp.Service = serviceType;
                        temp.Description= reader.GetString("Description");
                        Enum.TryParse(reader.GetString("TypeDish"), out typeDish);
                        temp.Type = typeDish;
                        listDishes.Add(temp);
                        temp = new Dish();
                    }
                }
            }
            return listDishes;
        }

        public bool DeleteMenuDetailsByMenuId(int menuId)
        {
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "DELETE FROM dbo.MenuDetails WHERE MenuId=@MenuId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("MenuId", menuId);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;

            }
            return success;
        }

        public bool DeleteMenuDetailsByDishId(int dishId)
        {
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "DELETE FROM dbo.MenuDetails WHERE DishId=@DishId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("DishId", dishId);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;

            }
            return success;
        }

        public List<Menu> GetMenus(Restaurant resto) {
            List<Menu> listMenus = new List<Menu>();
            Menu menu = new Menu();
            TypeService serviceType;
            decimal price;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string request = "SELECT MenuId,Name,Price,Description,TypeService FROM dbo.Menus WHERE RestaurantId=@RestaurantId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestaurantId", resto.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        menu.Id = reader.GetInt32("MenuId");
                        menu.Name = reader.GetString("Name");
                        price = reader.GetDecimal("Price");
                        menu.Price = (double)price;
                        Enum.TryParse(reader.GetString("TypeService"), out serviceType);
                        menu.Service = serviceType;
                        menu.Description = reader.GetString("Description");
                        listMenus.Add(menu);
                        menu = new Menu();
                    }
                }
            }
            foreach (var item in listMenus)
            {
                item.DishList = GetMenuDetails(item.Id);
            }
            return listMenus;
        }

        public Menu GetMenuById(int MenuId)
        {
            Menu menu = new Menu();
            TypeService serviceType;
            decimal price;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT MenuId,Name,Price,Description,TypeService FROM dbo.Menus WHERE MenuId=@MenuId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("MenuId", MenuId);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menu.Id = reader.GetInt32("MenuId");
                        menu.Name = reader.GetString("Name");
                        price = reader.GetDecimal("Price");
                        menu.Price = (double)price;
                        Enum.TryParse(reader.GetString("TypeService"), out serviceType);
                        menu.Service = serviceType;
                        menu.Description = reader.GetString("Description");
                    }
                }
            }   
            menu.DishList = GetMenuDetails(menu.Id);
            return menu;
        }
        public List<Dish> GetMenuDetails(int menuId)
        {
            List<Dish> MenuDetails = new List<Dish>();
            Dish Detail = new Dish();
            int id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT DishId FROM dbo.MenuDetails WHERE MenuId=@MenuId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("MenuId", menuId);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("DishId");
                        Detail = GetDishById(id);
                        MenuDetails.Add(Detail);
                        Detail = new Dish();
                    }
                }
            }
            return MenuDetails;
        }
        public Dish GetDishById(int id)
        {
            Dish SearchedDish = new Dish();
            TypeService serviceType;
            decimal price;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT * FROM dbo.Dishes WHERE DishId=@DishId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("DishId", id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SearchedDish.Id = reader.GetInt32("DishId");
                        SearchedDish.Name = reader.GetString("Name");
                        price = reader.GetDecimal("Price");
                        SearchedDish.Price = (double)price;
                        Enum.TryParse(reader.GetString("TypeService"), out serviceType);
                        SearchedDish.Service = serviceType;
                        SearchedDish.Description = reader.GetString("Description");
                    }
                }
            }
            return SearchedDish;
        }

        public bool DeleteMenuDetail(Menu menu, Dish dish)
        {
            string request = "DELETE FROM dbo.MenuDetails WHERE MenuId=@MenuId AND DishId=@DishId";
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("MenuId", menu.Id);
                cmd.Parameters.AddWithValue("DishId", dish.Id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
            }
            return success;
        }

        public bool AddingOneMenuDetail(Menu menu,Dish dish)
        {
            bool success = false;
            string request = "INSERT INTO dbo.MenuDetails(DishId,MenuId) VALUES (@DishId,@MenuId)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("DishId", dish.Id);
                cmd.Parameters.AddWithValue("MenuId", menu.Id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
                connection.Close();  
            }
            return success;
        }
        public Meal Delete(Meal meal) {
            if(meal is Menu) {
                Menu mealMenu = new Menu();
                mealMenu = (Menu)meal;

                DeleteMenuDetailsByMenuId(mealMenu.Id);
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    string request = "DELETE FROM dbo.Menus WHERE MenuId=@MenuId";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("MenuId", mealMenu.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                }
                return mealMenu;
            }
            if(meal is Dish) {
                Dish mealDish = new Dish();
                mealDish = (Dish)meal;

                DeleteMenuDetailsByDishId(meal.Id);
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    string request = "DELETE FROM dbo.Dishes WHERE DishId=@DishId";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("DishId", mealDish.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                }
                return mealDish;
            }
            return meal;
        }

        public bool Update(Meal meal) {
            bool success = false;
            if(meal is Menu) {

                Menu mealMenu = new Menu();
                mealMenu = (Menu)meal;

                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    string request = "UPDATE dbo.Menus SET Name=@Name ,Price=@Price ,TypeService=@TypeService ,Description=@Description  WHERE MenuId=@MenuId";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("Name", mealMenu.Name);
                    cmd.Parameters.AddWithValue("Price", mealMenu.Price);
                    cmd.Parameters.AddWithValue("TypeService", mealMenu.Service);
                    cmd.Parameters.AddWithValue("Description", mealMenu.Description);
                    cmd.Parameters.AddWithValue("MenuId", mealMenu.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }

                List<Dish> MenuDishesBeforeModification = GetMenuDetails(mealMenu.Id);
                List<int> MenuDishesIdBeforeModification = new List<int>();
                //récuperer les id des menusDetails de la db
                foreach (var itemDB in MenuDishesBeforeModification) {
                    MenuDishesIdBeforeModification.Add(itemDB.Id);
                }

                foreach (var item in mealMenu.DishList) {
                    //Quand on  rajoute un plat au menu
                    if (!MenuDishesIdBeforeModification.Contains(item.Id)) {
                        success=AddingOneMenuDetail(mealMenu, item);
                    }
                }
                //récuperer les id des menusDetails de menu
                List<int> MenuDishesId = new List<int>();
                foreach (var dish in mealMenu.DishList) {
                    MenuDishesId.Add(dish.Id);
                }

                foreach (var item in MenuDishesBeforeModification) {
                    // si on a supprimé un dish dans le menu 
                    if (!MenuDishesId.Contains(item.Id)) {
                        success=DeleteMenuDetail(mealMenu, item);
                    }
                }
            }else{

                Dish mealDish = new Dish();
                mealDish = (Dish)meal;

                string request = "UPDATE dbo.Dishes SET Name=@Name ,Price=@Price ,TypeService=@TypeService ,Description=@Description ,TypeDish=@TypeDish WHERE DishId=@DishId";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("Name", mealDish.Name);
                    cmd.Parameters.AddWithValue("Price", mealDish.Price);
                    cmd.Parameters.AddWithValue("TypeService", mealDish.Service);
                    cmd.Parameters.AddWithValue("Description", mealDish.Description);
                    cmd.Parameters.AddWithValue("TypeDish", mealDish.Type);
                    // Pour une raison que je ne comprends pas, l'ID du dish n'est pas récupéré (objet envoyé en paramètre pourtant?)
                    int dishId = mealDish.Id;
                    cmd.Parameters.AddWithValue("DishId", dishId);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }
                return success;
            }
            return success;
        }
        public bool Add(Meal meal,Restaurant restaurant) {
            bool success = false;

            if(meal is Menu) {

                Menu mealMenu = new Menu();
                mealMenu = (Menu)meal;

                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    string request = "INSERT INTO dbo.Menus(Name,Price,TypeService,Description,RestaurantId) VALUES (@Name,@Price,@TypeService,@Description,@RestaurantId)";
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("Name", mealMenu.Name);
                    cmd.Parameters.AddWithValue("Price", mealMenu.Price);
                    cmd.Parameters.AddWithValue("TypeService", mealMenu.Service);
                    cmd.Parameters.AddWithValue("Description", mealMenu.Description);
                    cmd.Parameters.AddWithValue("RestaurantId", restaurant.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }
                //recuperer id du menu créé
                mealMenu.Id = GetMenuIdByName(mealMenu);
                //ajouter dans menu details aussi les dish composant le menu
                success = AddingMenuDetails(mealMenu);
                return success;
            }else{

                Dish mealDish = new Dish();
                mealDish = (Dish)meal;

                string request = "INSERT INTO dbo.Dishes(Name,Price,TypeService,Description,TypeDish,RestaurantId) VALUES (@Name,@Price,@TypeService,@Description,@TypeDish,@RestaurantId)";
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    SqlCommand cmd = new SqlCommand(request, connection);
                    cmd.Parameters.AddWithValue("Name", mealDish.Name);
                    cmd.Parameters.AddWithValue("Price", mealDish.Price);
                    cmd.Parameters.AddWithValue("TypeService", mealDish.Service);
                    cmd.Parameters.AddWithValue("Description", mealDish.Description);
                    string typeDish = mealDish.Type.ToString();
                    cmd.Parameters.AddWithValue("TypeDish", mealDish.Type);
                    cmd.Parameters.AddWithValue("RestaurantId", restaurant.Id);
                    connection.Open();
                    int res = cmd.ExecuteNonQuery();
                    success = res > 0;
                }
                return success;
            }
        }
    }
}