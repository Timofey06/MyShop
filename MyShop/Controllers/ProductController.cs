using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Data;
using MyShop.Data;
using MyShop.Models;
using MyShop.Models.ViewModels;

namespace ShopM4.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product;

            //получаем ссылки на сущности категорий
            foreach (var item in objList)
            {
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {

            /* IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x => new SelectListItem
             {
                 Text = x.Name,
                 Value = x.Id.ToString()
             });
             //получаем лист категорий для отправки его во view
             //ViewBag.CategoriesList = CategoriesList;
             ViewData["CategoriesList"] = CategoriesList;*/
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product=new Product(),
                CategoriesList = db.Category.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = db.Product.Find(id);
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }
    }
}
