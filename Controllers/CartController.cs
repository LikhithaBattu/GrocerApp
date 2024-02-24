using GrocerApp.Models;
using GrocerApp.ViewModel;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Product = Stripe.Product;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace GrocerApp.Controllers
{
    public class CartController : Controller
    {
        private GroceryDatabaseEntities db = new GroceryDatabaseEntities();
        private List<CartItemViewModel> cart = new List<CartItemViewModel>();
        private string base64EncryptedCreditCard;

        // Add an item to the cart
        public ActionResult AddToCart(int productId, int quantity)
        {
            // Retrieve the product based on the productId
            var product = db.Products.Find(productId); 

            if (product != null)
            {
                var cart = Session["Cart"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();

                // Check if the product is already in the cart
                var existingCartItem = cart.FirstOrDefault(item => item.ProductId == productId);

                if (existingCartItem != null)
                {
                    // If the product is in the cart, update its quantity
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    // If the product is not in the cart, add it
                    var newCartItem = new CartItemViewModel
                    {
                        ProductId = productId,
                        ProductImageURL = product.ProductImageURL,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity
                    };
                    cart.Add(newCartItem);
                }

                Session["Cart"] = cart;
                UpdateCartCountInSession(cart);
            }
            
            // Redirect back to the cart page or any other appropriate page
            return RedirectToAction("Index");
        }

        // Display the cart contents on the Index page
        public ActionResult Index()
        {
            var cart = Session["Cart"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();
            var baseViewModel = new GrocerApp.ViewModel.BaseViewModel
            {
                CartItems = cart
            };
            ViewBag.Categories = db.Categories.ToList();
           
            // Pass the cart items to the view
            return View(baseViewModel);
        }
        public ActionResult RemoveFromCart(int productId)
        {
            var cart = Session["Cart"] as List<CartItemViewModel>;
            ViewBag.Categories = db.Categories.ToList();

            if (cart != null)
            {
                // Find the product in the cart
                var productToRemove = cart.FirstOrDefault(item => item.ProductId == productId);

                if (productToRemove != null)
                {
                    // Remove the product from the cart
                    cart.Remove(productToRemove);
                }

                // Update the session with the modified cart
                Session["Cart"] = cart;
                UpdateCartCountInSession(cart);
            }
           

            return RedirectToAction("Index");
        }
        private void UpdateCartCountInSession(List<CartItemViewModel> cart)
        {
            // Calculate total quantity
            int totalQuantity = cart.Sum(item => item.Quantity);

            // Update cart count in session
            Session["CartCount"] = totalQuantity;
        }
        [HttpPost]
        public ActionResult UpdateCartItemQuantity(int productId, int quantity)
        {
            var cart = Session["Cart"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();
            ViewBag.Categories = db.Categories.ToList();

            // Find the item in the cart
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }

            // Update the cart count in the session
            int cartCount = cart.Sum(item => item.Quantity);
            Session["CartCount"] = cartCount;

            return Json(new { cartCount }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();
            var baseViewModel = new GrocerApp.ViewModel.BaseViewModel
            {
                CartItems = cart
            };
            ViewBag.Categories = db.Categories.ToList();

            // Pass the cart items to the view
            return View(baseViewModel);
        }

        [HttpPost]
        public ActionResult ProcessCheckout(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the user is authenticated
                    if (Session["user"] != null && Session["user"] is GrocerApp.Models.User)
                    {

                        var userFromSession = (GrocerApp.Models.User)Session["user"];

                        var db = new GroceryDatabaseEntities(); 
                        

                        if (userFromSession.Username != null)
                        {
                            var cart = Session["Cart"] as List<CartItemViewModel> ?? new List<CartItemViewModel>();
                            var cartItemList = cart;
                           
                            // User information found, use it to create the new order
                            var newOrder = new Order
                            {
                                UserID = userFromSession.UserID, // Assign the UserId
                                OrderDate = DateTime.Now,
                                TotalCost = model.OrderTotal,
                                Address = model.Address,
                                City = model.City,
                                State = model.State,
                                ZipCode = model.ZipCode
                            };
                                db.Orders.Add(newOrder);
                                db.SaveChanges();
                            var orderNumber = newOrder.OrderNumber;
                                                       
                            var paymentInfo = new PaymentInformation
                            {
                                OrderNumber = newOrder.OrderNumber,
                               EncryptedPaymentDetails= model.cardNumber,
                                PaymentAmount = model.OrderTotal,
                                UserID= userFromSession.UserID,
                                PaymentMethod = model.PaymentMethod
                                
                            };

                                db.PaymentInformations.Add(paymentInfo);
                                db.SaveChanges();

                            // Save order details in the OrderItems table
                            foreach (var item in cartItemList)
                            {
                                // Decrease stock level in Inventory based on the quantity selected
                                var inventoryItem = db.Inventories.FirstOrDefault(i => i.ProductID == item.ProductId);
                                if (inventoryItem != null)
                                {
                                    inventoryItem.StockLevel -= item.Quantity;
                                }

                                // Save order item details
                                var orderItem = new OrderItem
                                {
                                    OrderNumber = newOrder.OrderNumber,
                                    ProductID = item.ProductId,
                                    Quantity = item.Quantity
                                };

                                db.OrderItems.Add(orderItem);
                            }

                            db.SaveChanges();
                            Session["Cart"] = new List<CartItemViewModel>();
                            Session["CartCount"] = 0;
                            UpdateCartCountInSession(new List<CartItemViewModel>());

                            // Redirect to the success page
                            return RedirectToAction("OrderSuccess", new { orderNumber = newOrder.OrderNumber });
                        }
                        else
                        {
                            // Handle the case where the user information is not found
                            ModelState.AddModelError("", "User information not found.");
                            return View("Checkout", model);
                        }
                    }
                    else
                    {
                        // Handle the case where the user is not authenticated
                        ModelState.AddModelError("", "User is not authenticated.");
                        return View("Checkout", model);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions, log them, and return an error view or message
                    ModelState.AddModelError("", "An error occurred while processing your order.");
                }
            }

            // If there are validation errors, return to the checkout page with errors
            return View("Checkout", model);
        }
        public ActionResult OrderSuccess(string orderNumber)
        {
            // Display the order confirmation number on the success page
            ViewBag.OrderNumber = orderNumber;
            ViewBag.Categories = db.Categories.ToList();

            return View();
        }
      



    }
}
