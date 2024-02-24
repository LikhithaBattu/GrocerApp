using GrocerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GrocerApp.Controllers
{
    public class AccountController : Controller
    {
        private GroceryDatabaseEntities1 db = new GroceryDatabaseEntities1();
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if already logged in.
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    // Authentication successful, create an authentication cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    return RedirectToAction("Index", "Home"); // Redirect to a protected page
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Register()
        {
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
                    City=model.City,
                    State=model.State,
                    ZipCode=model.ZipCode
                };

                // Add the user to the database and save changes
                using (var context = new GroceryDatabaseEntities1())
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


    }
}