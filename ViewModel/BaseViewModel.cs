using GrocerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrocerApp.ViewModel
{
    public class BaseViewModel
    {
        public List<Category> Categories { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
        public List<Order> Orders { get; set; }
        public User User { get; set; }
        public List<User> Users { get; set; }
        public List<Product> Products { get; set; }
        public List<Inventory> Inventory { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public bool IsEditMode { get; set; }
    }

    public class HomeViewModel : BaseViewModel
    {
        
    }

    public class LoginViewModel:BaseViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class CategoryDetailsViewModel:BaseViewModel
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
    public class ResetPasswordModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ProductDetailsViewModel : BaseViewModel
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string ProductImageURL { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DetailDescription { get; set; }
        public string Ingredients { get; set; }
        public string NutritionalInfo { get; set; }
        public string SafetyInfo { get; set; }
    }
    public class CartItemViewModel:BaseViewModel
    {
        public int ProductId { get; set; }
        public string ProductImageURL { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Count => Quantity;
        public string DetailDescription { get; set; }
        public string Ingredients { get; set; }
        public string NutritionalInfo { get; set; }
        public string SafetyInfo { get; set; }
    }
    public class CartViewModel : BaseViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public CartViewModel()
        {
            CartItems = new List<CartItemViewModel>();
        }
    }
    public class CheckoutModel:BaseViewModel
    {
         public string Name { get; set; }

 
        public string Address { get; set; }

 
        public string City { get; set; }


        public string State { get; set; }

 
        public string ZipCode { get; set; }

        public string PaymentMethod { get; set; }
        public decimal OrderTotal { get; set; }
        public int UserID { get; set; }
        public string StripeToken { get; set; }
        public string cardNumber { get; set; }
        public string cardExpiryMonth { get; set; }
        public string cardExpiryYear { get; set; }
        public string cardCVC { get; set; }

        public List<CartItemViewModel> CartItems { get; set; }
    }


}