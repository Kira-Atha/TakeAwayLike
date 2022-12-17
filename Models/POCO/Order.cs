using ASP_PROJECT.DAL.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASP_PROJECT.Models.POCO {
    public enum OrderStatus {
        Validate,
        OnPreparation,
        //OnDelivery,
        Finished
    }

    public class Order {

        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
        public OrderStatus Status { get; set; }
        //public bool Delivery { get; set; }

        [Required(ErrorMessage = "Champs obligatoire")]
        [Display(Name = "Adresse de livraison")]
        public string DeliveryAdress { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Prix total")]
        public double TotalPrice { get; set; }

        [Display(Name = "Date de commande")]
        public DateTime DateOrder { get; set; }
        public List<Meal> ListMealOrdered { get; set; }

        public Order() {
            ListMealOrdered = new List<Meal>();
        }
        public Order(Restaurant resto, Customer customer) : this() {
            Restaurant = resto;
            Customer = customer;
            // La liste pourrait être vide ici !! 
            

            // Pas de constructeur par défaut, pas utile, car ça veut dire que la commande n'appartient à ni un resto ni à un customer !!
            // Ou alors je cr'éé un faux mais ça n'a AUCUN SENS.
        }
        public string ConvertOrderStatus() {
            string value = "";
            switch (this.Status) {
                case OrderStatus.Validate:
                    value = "Validée";
                    break;

                case OrderStatus.OnPreparation:
                    value = "En préparation";
                    break;

                case OrderStatus.Finished:
                    value = "Terminée";
                    break;
            }
            return value;
        }

        public void CalculateTotalPrice() {

            // MIEUX : 
            foreach (var item in this.ListMealOrdered) {
                this.TotalPrice += item.Price;
            }

            // Si le prix est calculé différemment pour le dish !!! Propriété commune, c'est idiot de le faire comme ça !!

            //double totalDishes = 0;
            //foreach (var element in this.ListMealOrdered) {
            //    if (element is Dish)
            //        totalDishes += element.Price;
            //}
            //double totalMenus = 0;
            //foreach (var element in this.ListMealOrdered) {
            //    if (element is Menu)
            //        totalMenus += element.Price;
            //}
            //this.TotalPrice = totalDishes + totalMenus;
        }

        public bool ValidateOrder(IOrderDAL orderDAL) {
            Customer.OrdersList.Add(this);
            return orderDAL.AddOrder(this);
        }

        public static Order GetOrderById(IOrderDAL orderDAL, int orderId) {
            return orderDAL.GetOrderById(orderId);
        }

        public bool UpdateOrderStatus(IOrderDAL orderDAL) {
            return orderDAL.UpdateOrderStatus(this);
        }
    }
}
