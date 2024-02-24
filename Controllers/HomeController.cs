using GrocerApp.Models;
using GrocerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrocerApp.Controllers
{
    public class HomeController : Controller
    {
        private GroceryDatabaseEntities db = new GroceryDatabaseEntities();
        public ActionResult Index()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }

        public ActionResult AboutUs()
        {

            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;

            return View();
        }

        public ActionResult ContactUs()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }

        public ActionResult ReturnPolicy()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }

        public ActionResult TermsOfService()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }
        public ActionResult FAQs()
        {
            var Categories = db.Categories.ToList();
            ViewBag.Categories = Categories;
            return View();
        }
        public ActionResult Search(string searchQuery)
        {
            // Perform a database query to search for products based on the searchQuery
            var db = new GroceryDatabaseEntities(); 
            var Categories = db.Categories.ToList();
            var products = db.Products.Where(p => p.Name.Contains(searchQuery)).ToList();
            var viewModel = new CategoryDetailsViewModel
            {
                Products = products
            };

          

            return View(viewModel);
        }

    }
}