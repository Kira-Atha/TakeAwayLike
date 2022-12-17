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
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;

        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Order> GetRestaurantOrders(Restaurant restaurant)
        {
            decimal price;
            List<Order> RestaurantOrders = new List<Order>();
            Order order = new Order();
            OrderStatus Status;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT * FROM dbo.Orders WHERE RestaurantId=@RestaurantId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("RestaurantId", restaurant.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        order.Restaurant = restaurant;
                        order.Id = reader.GetInt32("OrderId");
                        order.DeliveryAdress = reader.GetString("DeliveryAdress");
                        price = reader.GetDecimal("Price");
                        order.TotalPrice = (double)price;
                        order.Customer.Id= reader.GetInt32("CustomerId");
                        order.DateOrder = reader.GetDateTime("OrderDate");
                        Enum.TryParse(reader.GetString("OrderStatus"), out Status);
                        order.Status = Status;

                        //get Order Dish and Menu Details
                        RestaurantOrders.Add(order);
                        order = new Order();

                    }
                }
            }
            return RestaurantOrders;
        }

        public List<Order> GetCustomerOrders(Customer customer)
        {
            decimal price;
            List<Order> CustomerOrders = new List<Order>();
            Order order = new Order();
            order.Customer = customer;
            OrderStatus Status;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT * FROM dbo.Orders WHERE CustomerId=@CustomerId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("CustomerId", customer.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        order.Customer = customer;
                        order.Restaurant.Id= reader.GetInt32("RestaurantId");
                        order.Id = reader.GetInt32("OrderId");
                        order.DeliveryAdress = reader.GetString("DeliveryAdress");
                        price = reader.GetDecimal("Price");
                        order.TotalPrice = (double)price;
                        order.Customer.Id = reader.GetInt32("CustomerId");
                        order.DateOrder = reader.GetDateTime("OrderDate");
                        Enum.TryParse(reader.GetString("OrderStatus"), out Status);
                        order.Status = Status;
                        CustomerOrders.Add(order);
                            // OU BIEN :
                            //customer.OrdersList.Add(Order); sans recréer de liste
                            // Ici, La OrdersList est mise à jour dans la POCO grâce à ce qui est récupéré
                        order = new Order();
                    }
                }
            }
            return CustomerOrders;
        }


        public List<int> GetMenusIdInMenuDetails(Order order)
        {
            List<int> MenuDetailsId = new List<int>();
            int id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT MenuId FROM dbo.OrderMenuDetails WHERE OrderId=@OrderId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("OrderId", order.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("MenuId");
                        MenuDetailsId.Add(id);
                    }
                }
            }
            return MenuDetailsId;
        }

        public List<int> GetDishesIdInMenuDetails(Order order)
        {
            List<int> DishDetailsId = new List<int>();
            int id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT DishId FROM dbo.OrderDishDetails WHERE OrderId=@OrderId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("OrderId", order.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("DishId");
                        DishDetailsId.Add(id);
                    }
                }
            }
            return DishDetailsId;
        }

        public bool AddOrder(Order order)
        {
            Customer customer = order.Customer;

            string request = "INSERT INTO dbo.Orders (OrderStatus,OrderDate,DeliveryAdress,CustomerId,Price,RestaurantId) VALUES (@OrderStatus,@OrderDate,@DeliveryAdress,@CustomerId,@Price,@RestaurantId)";
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("OrderStatus", order.Status);
                cmd.Parameters.AddWithValue("OrderDate", order.DateOrder.ToString("MM/dd/yyyy"));
                cmd.Parameters.AddWithValue("DeliveryAdress", order.DeliveryAdress);
                cmd.Parameters.AddWithValue("CustomerId", customer.Id);
                cmd.Parameters.AddWithValue("Price", order.TotalPrice);
                cmd.Parameters.AddWithValue("RestaurantId", order.Restaurant.Id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;

                //recuper l'id de l'order qu'on d'ajouté
                order.Id = GetOrderId(order);

                foreach (var meal in order.ListMealOrdered)
                {
                    if (meal is Menu) {
                        success = AddOrderMenuDetails(order, meal as Menu);
                    } else {
                        success = AddOrderDishDetails(order, meal as Dish);
                    }
                }
                return success;
            }
        }
        public bool AddOrderMenuDetails(Order order, Menu menu)
        {
            string request = "INSERT INTO dbo.OrderMenuDetails (OrderId,MenuId) VALUES (@OrderId,@MenuId)";
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(request, connection);

                cmd.Parameters.AddWithValue("OrderId", order.Id);
                cmd.Parameters.AddWithValue("MenuId", menu.Id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;

                return success;
            }
        }
        public bool AddOrderDishDetails(Order order, Dish dish)
        {
            string request = "INSERT INTO dbo.OrderDishDetails (OrderId,DishId) VALUES (@OrderId,@DishId)";
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(request, connection);

                cmd.Parameters.AddWithValue("OrderId", order.Id);
                cmd.Parameters.AddWithValue("DishId", dish.Id);

                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;

                return success;
            }
        }

        public int GetOrderId(Order order)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT OrderId FROM dbo.Orders WHERE  Price=@Price AND RestaurantId=@RestaurantId AND CustomerId=@CustomerId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("Price", order.TotalPrice);
                cmd.Parameters.AddWithValue("RestaurantId", order.Restaurant.Id);
                cmd.Parameters.AddWithValue("CustomerId", order.Customer.Id);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32("OrderId");
                    }
                }
            }
            return id;
        }

        public Order GetOrderById(int orderId)
        {
            Order SearchedOrder = new Order();
            OrderStatus Status;
            decimal price;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string request = "SELECT * FROM dbo.Orders WHERE OrderId=@OrderId";
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("OrderId", orderId);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        SearchedOrder.Id = orderId;
                        SearchedOrder.Restaurant.Id = reader.GetInt32("RestaurantId");
                        SearchedOrder.DeliveryAdress = reader.GetString("DeliveryAdress");
                        price = reader.GetDecimal("Price");
                        SearchedOrder.TotalPrice = (double)price;
                        SearchedOrder.Customer.Id = reader.GetInt32("CustomerId");
                        SearchedOrder.DateOrder = reader.GetDateTime("OrderDate");
                        Enum.TryParse(reader.GetString("OrderStatus"), out Status);
                        SearchedOrder.Status = Status;
                    }
                }
            }
            return SearchedOrder;
        }

        public bool UpdateOrderStatus(Order Order)
        {
            string request = "UPDATE dbo.Orders SET OrderStatus=@OrderStatus  WHERE OrderId=@OrderId";
            bool success = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(request, connection);
                cmd.Parameters.AddWithValue("OrderStatus", Order.Status);
                cmd.Parameters.AddWithValue("OrderId", Order.Id);
                connection.Open();
                int res = cmd.ExecuteNonQuery();
                success = res > 0;
            }

            return success;
        }
    }
}
