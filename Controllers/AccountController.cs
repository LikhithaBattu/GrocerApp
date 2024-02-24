using GrocerApp.Models;
using GrocerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GrocerApp.Controllers
{
    public class AccountController : Controller
    {
        private GroceryDatabaseEntities db = new GroceryDatabaseEntities();
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            var viewModel = new LoginViewModel();
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if already logged in.
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var db = new GroceryDatabaseEntities();
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Authentication successful, create an authentication cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    Session["user"] = user;
                    Debug.WriteLine("Authentication Status: " + User.Identity.IsAuthenticated);

                    return RedirectToAction("Index", "Home"); // Redirect to a protected page
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            // Clear the user session
            Session["user"] = null;

            // Redirect to the login page
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult Register()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Create a new User object and map the values from the registration view model
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    City= model.City,
                    State= model.State,
                    ZipCode= model.ZipCode,
                    RoleCode= "USR"
                };

                // Add the user to the database and save changes
                using (var context = new GroceryDatabaseEntities())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                // Redirect to a success page or login page
                return RedirectToAction("Login", "Account");
            }

            // If registration fails, return to the registration page with validation errors
            return View(model);
        }

        [HttpGet]
        public ActionResult RecoveryPassword()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new GroceryDatabaseEntities(); 
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null)
                {
                    user.Password = model.Password;

                    db.SaveChanges(); // Save the updated password

                    // Redirect to a success page or login page
                    return RedirectToAction("Login");
                }
                else
                {
                    // Email doesn't exist, add a validation error
                    ModelState.AddModelError("Email", "Email does not exist.");
                }
            }

            // If there are validation errors or the email doesn't exist, return to the reset password page
            return View("RecoveryPassword", model);
        }
        [HttpGet]
        public ActionResult OrderHistory()
        {
            if (Session["user"] != null && Session["user"] is GrocerApp.Models.User)
            {
                var user = (GrocerApp.Models.User)Session["user"];
                var db = new GroceryDatabaseEntities(); 
                var Categories = db.Categories.ToList();
                ViewBag.Categories = Categories;

                // Retrieve order history for the user
                var orderHistory = db.Orders
                    .Where(o => o.UserID == user.UserID)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                var viewModel = new BaseViewModel
                {
                    Orders = orderHistory
                };

                return View(viewModel);
            }

            // Handle the case where the user is not logged in
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult AccountDetails()
        {
            if (Session["user"] != null && Session["user"] is GrocerApp.Models.User)
            {
                var user = (GrocerApp.Models.User)Session["user"];
                var db = new GroceryDatabaseEntities(); 
                var Categories = db.Categories.ToList();
                ViewBag.Categories = Categories;
                // Retrieve user details from the database
                var userDetails = db.Users.SingleOrDefault(u => u.UserID == user.UserID);
                var viewModel = new BaseViewModel
                {
                    User = userDetails
                };

                if (userDetails != null)
                {
                    return View(viewModel); // Pass the user details to the view
                }
            }

            // Handle the case where the user is not logged in or user details are not found
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult OrderDetails(int orderNumber)
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            // Retrieve order items associated with the given orderNumber from the OrderItem table
            var orderItems = db.OrderItems
                                .Where(oi => oi.OrderNumber == orderNumber)
                                .Include(oi => oi.Product) // Include the related Product information
                                .ToList();

            // Create and populate the BaseViewModel
            var viewModel = new BaseViewModel
            {
                OrderItems = orderItems,
            };

            // Pass the view model to the view
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateUserDetails(string Address, string City, string State, string ZipCode)
        {
            try
            {
                // Get the current user from your session or identity
                var currentUser = GetCurrentUser(); 

                // Retrieve the user from the database using the user's ID or unique identifier
                var db = new GroceryDatabaseEntities(); 
                var user = db.Users.Find(currentUser.UserID); 

                if (user != null)
                {
                    // Update user details
                    user.Address = Address;
                    user.City = City;
                    user.State = State;
                    user.ZipCode = ZipCode;

                    // Save changes to the database
                    db.SaveChanges();

                    // Return success
                    return Json(new { success = true });
                }

                // Return failure if the user is not found
                return Json(new { success = false, message = "User not found." });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return Json(new { success = false, message = "An error occurred while updating user details." });
            }
        }

       
        private User GetCurrentUser()
        {
            // Your logic to retrieve the current user (from session, identity, etc.)
            // This is just a placeholder, replace it with your actual implementation
            return new User { UserID = 1 }; // Example: Return a user with UserId 1
        }
        [HttpPost]
        public ActionResult SaveCustomerSupport(string name, string email, string message)
        {
           
            using (var db = new GroceryDatabaseEntities())
            {
                var Categories = db.Categories.ToList();
                ViewBag.Categories = Categories;
                // Create a new CustomerSupport object
                var customerSupport = new CustomerSupport
                {
                    CustomerName = name,
                    CustomerEmail = email,
                    CustomerMessage = message
                };

                // Add to the context and save changes
                db.CustomerSupports.Add(customerSupport);
                db.SaveChanges();
            }

            // Redirect to the thank you page
            return Json(new { success = true });
        }


        public ActionResult ThankYou()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View(); 
        }


    }
}