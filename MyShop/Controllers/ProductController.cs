using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop;
using MyShop.Data;
using MyShop.Data;
using MyShop.Models;
using MyShop.Models.ViewModels;

namespace ShopM4.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;
        private IWebHostEnvironment env;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product;

            //получаем ссылки на сущности категорий
            //foreach (var item in objList)
            //{
            //    item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            //}
            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Category.Select(x =>
                new SelectListItem
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

        //Post -CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit( ProductViewModel productViewModel)
        {
            var files=HttpContext.Request.Form.Files;

            string wwRoot = env.WebRootPath;

            if (productViewModel.Product.Id==0)
            {
                //create
                string upload = wwRoot +"/"+ PathManager.ImageProductPass;
                string imageName=Guid.NewGuid().ToString();
                string extansion = Path.GetExtension(files[0].FileName);
                string path = upload + imageName + extansion;
                //copy file to server
                var fileStream = new FileStream(path, FileMode.Create);
                
                files[0].CopyTo(fileStream);

                productViewModel.Product.Image = imageName + extansion;
                db.Product.Add(productViewModel.Product);
            }
            else
            {
                //update
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
