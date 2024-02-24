using GrocerApp.Models;
using GrocerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrocerApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private GroceryDatabaseEntities db = new GroceryDatabaseEntities();
        public ActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            var baseViewModel = new BaseViewModel
            {
                Users = db.Users.ToList(),             
                Products = db.Products.ToList(),
                Orders = db.Orders.ToList(),
                Inventory = db.Inventories.ToList()
            };
            return View(baseViewModel);
        }
        [HttpGet]
        public ActionResult Users()
        {
            // Retrieve the list of users from the database (replace this with your actual logic)
            var userList = db.Users.ToList();
            ViewBag.Categories = db.Categories.ToList();

            // Pass the user list to the view
            return View(userList);
        }
        public ActionResult EditUser(int userId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            // Retrieve user details by userId
            var user = db.Users.Find(userId);

            // Create a BaseViewModel and set the Users property
            var baseViewModel = new BaseViewModel
            {
                Users = new List<User> { user } // Assuming Users is a List<User> property in BaseViewModel
                                                
            };

            return View(baseViewModel);
        }


        [HttpPost]
        public ActionResult EditUser(User editedUser)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            // Retrieve the existing user from the database
            var existingUser = db.Users.Find(editedUser.UserID);

            if (existingUser != null)
            {
                // Update user details
                existingUser.Username = editedUser.Username;
                existingUser.Email = editedUser.Email;

                if (string.IsNullOrWhiteSpace(existingUser.Username))
                {
                    ModelState.AddModelError("Username", "Username is required.");
                }

                if (!IsValidEmail(existingUser.Email))
                {
                    ModelState.AddModelError("Email", "Invalid email address.");
                }

                if (ModelState.IsValid)
                {
                    // Save changes to the database
                    db.SaveChanges();

                    // Set a success message
                    TempData["SuccessMessage"] = "User details updated successfully!";
                }
            }

            // Redirect back to the Admin Dashboard
            return RedirectToAction("Index");
        }

        // Helper function to check if the email is valid
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        public ActionResult DeleteUser(int userId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            // Retrieve user details by userId
            var user = db.Users.Find(userId);

            if (user != null)
            {
                // Remove the user from the database
                db.Users.Remove(user);

                // Save changes to the database
                db.SaveChanges();
                TempData["SuccessMessage"] = "User details Deleted successfully!";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }
        public ActionResult EditProduct(int productId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.Categories = db.Categories.ToList();

            // Retrieve product details by productId
            var product = db.Products.Find(productId);
            var baseViewModel = new BaseViewModel
            {
                Products = new List<Product> { product } 

            };

          

            return View(baseViewModel);
        }

        [HttpPost]
        public ActionResult EditProduct(Product editedProduct)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            var existingProduct = db.Products.Find(editedProduct.ProductID);

            if (existingProduct != null)
            {
                // Update product details
                existingProduct.Name = editedProduct.Name;
                existingProduct.Description = editedProduct.Description;

                // Save changes to the database
                db.SaveChanges();

                // Set success message for the view
                TempData["SuccessMessage"] = "Product details updated successfully.";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }


        public ActionResult DeleteProduct(int productId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            // Retrieve product details by productId
            var product = db.Products.Find(productId);
            if (product != null)
            {
                // Remove the user from the database
                db.Products.Remove(product);

                // Save changes to the database
                db.SaveChanges();
                TempData["SuccessMessage"] = "Product details Deleted successfully!";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }
        public ActionResult EditOrder(int orderNumber)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.Categories = db.Categories.ToList();

            // Retrieve inventory details by productId
            var order = db.Orders.Find(orderNumber);
            var baseViewModel = new BaseViewModel
            {
                Orders = new List<Order> { order } 

            };

           

            return View(baseViewModel);
        }

        [HttpPost]
        public ActionResult EditOrder(Order editedOrder)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            var existingOrder = db.Orders.Find(editedOrder.OrderNumber);

            if (existingOrder != null)
            {
                // Update product details
                existingOrder.OrderNumber = editedOrder.OrderNumber;
                existingOrder.TotalCost = editedOrder.TotalCost;
                existingOrder.Address = editedOrder.Address;
                existingOrder.City = editedOrder.City;
                existingOrder.State = editedOrder.State;
                existingOrder.ZipCode = editedOrder.ZipCode;

                // Save changes to the database
                db.SaveChanges();

                // Set success message for the view
                TempData["SuccessMessage"] = "Order details updated successfully.";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }


        public ActionResult DeleteOrder(int orderNumber)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            // Retrieve product details by productId
            var order = db.Orders.Find(orderNumber);

            if (order != null)
            {
                // Remove the user from the database
                db.Orders.Remove(order);

                // Save changes to the database
                db.SaveChanges();
                TempData["SuccessMessage"] = "Order details Deleted successfully!";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }
        public ActionResult EditInventory(int productId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            ViewBag.Categories = db.Categories.ToList();

            // Retrieve inventory details by productId
            var inventory = db.Inventories.Find(productId);
            var baseViewModel = new BaseViewModel
            {
                Inventory = new List<Inventory> { inventory } 

            };

           

            return View(baseViewModel);
        }

        [HttpPost]
        public ActionResult EditInventory(Inventory editedInventory)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            var existingInventory = db.Inventories.Find(editedInventory.ProductID);

            if (existingInventory != null)
            {
                // Update product details
                existingInventory.ProductID = editedInventory.ProductID;
                existingInventory.StockLevel = editedInventory.StockLevel;

                // Save changes to the database
                db.SaveChanges();

                // Set success message for the view
                TempData["SuccessMessage"] = "Inventory details updated successfully.";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }


        public ActionResult DeleteInventory(int productId)
        {
            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            // Retrieve product details by productId
            var inventory = db.Inventories.Find(productId);

            if (inventory != null)
            {
                // Remove the user from the database
                db.Inventories.Remove(inventory);

                // Save changes to the database
                db.SaveChanges();
                TempData["SuccessMessage"] = "Inventory details Deleted successfully!";
            }

            return RedirectToAction("Index"); // Redirect back to the Admin Dashboard
        }


    }
}