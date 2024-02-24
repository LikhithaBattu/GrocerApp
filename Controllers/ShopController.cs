using GrocerApp.Models;
using GrocerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrocerApp.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        private GroceryDatabaseEntities db = new GroceryDatabaseEntities();
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }
        public ActionResult Category(int categoryId)
        {
            var category = db.Categories.Find(categoryId); 
            ViewBag.Categories = db.Categories.ToList();
            if (category == null)
            {
                return HttpNotFound();
            }
            var products = new List<Product>();

            if (categoryId == 1)
            {
                // If categoryId is 1, retrieve all products without filtering by category
                products = db.Products.ToList();
            }
            else
            {
                // Retrieve products based on the specified categoryId
                products = db.Products.Where(p => p.CategoryID == categoryId).ToList();
            }

            var viewModel = new CategoryDetailsViewModel
            {
                Category = category,
                Products = products
            };



            return View(viewModel);
        }

        public ActionResult ProductDetail(int productId)
        {
            // Fetch the product details based on the productId
            var product = db.Products.Find(productId);
            var viewModel = new ProductDetailsViewModel
            {
                ProductID = product.ProductID,
                Product = product,
                Name = product.Name, 
                Description = product.Description, 
                Price = product.Price,
                ProductImageURL = product.ProductImageURL, 
                DetailDescription = product.DetailDescription,
                Ingredients = product.Ingredients,
                NutritionalInfo = product.NutritionalInfo,
                SafetyInfo = product.SafetyInfo
            };
            ViewBag.Categories = db.Categories.ToList();
            return View("ProductDetail", viewModel);
        }



    }
}