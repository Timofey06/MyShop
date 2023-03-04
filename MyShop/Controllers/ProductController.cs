using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop;
using MyShop_DataMigrations;
using MyShop_DataMigrations;
using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models;
using MyShop_Models.ViewModels;
using MyShop_Utility;

namespace ShopM4.Controllers
{

    //[Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        private IRepositoryProduct repositoryProduct;
        
        private IWebHostEnvironment env;

        public ProductController(IRepositoryProduct repositoryProduct, IWebHostEnvironment env)
        {
            this.repositoryProduct=repositoryProduct;
            this.env = env;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = repositoryProduct.GetAll();

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
                CategoriesList = repositoryProduct.GetListItems(PathManager.NameCategory),
                MyModelList= repositoryProduct.GetListItems(PathManager.NameMyModel)
            };

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = repositoryProduct.Find(id.GetValueOrDefault());

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
                repositoryProduct.Add(productViewModel.Product);
                fileStream.Close();
            }
            else
            {
                //update
                var product= repositoryProduct.FirstOrDefault(u=>u.Id==productViewModel.Product.Id, IsTracing:false);
                if (files.Count>0)
                {
                    string upload = wwRoot + "/" + PathManager.ImageProductPass;
                    string imageName = Guid.NewGuid().ToString();
                    string extansion = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extansion;

                    var oldFile =upload+product.Image;
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    var fileStream = new FileStream(path, FileMode.Create);

                    files[0].CopyTo(fileStream);
                    

                    productViewModel.Product.Image=imageName + extansion;
                    fileStream.Close();

                }
                else
                {
                    productViewModel.Product.Image = product.Image;
                }
                repositoryProduct.Update(productViewModel.Product);
            }
            repositoryProduct.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id==null||id==0)
            {
                return NotFound();
            }
            Product product = repositoryProduct.FirstOrDefault
                (x=>x.Id==id, includeProperties: "Category,MyModel");
            if (product == null)
            {
                return NotFound();
            }
            //product.Category = db.Category.Find(product.CategoryId);
            if (product.Category==null)
            {
                return NotFound();
            }
            //product.MyModel = db.MyModel.Find(product.MyModelId);
            if (product.MyModel == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            string wwRoot = env.WebRootPath;
            Product product = repositoryProduct.Find(id.GetValueOrDefault());
            if (product==null)
            {
                return NotFound();
            }
           
           
            string upload = wwRoot + "/" + PathManager.ImageProductPass;
            var oldFile = upload + product.Image;
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            repositoryProduct.Remove(product);
            repositoryProduct.Save();
            return RedirectToAction("Index");
        }
    }
}
