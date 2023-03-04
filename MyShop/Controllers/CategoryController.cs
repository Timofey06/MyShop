using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop_DataMigrations;
using MyShop_Models;
using MyShop_DataMigrations.Repository.IRepository;


namespace MyShop.Controllers
{
    //[Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        //private ApplicationDbContext db;
        IRepositoryCategory repositoryCategory;
        public CategoryController(IRepositoryCategory repositoryCategory )
        {
            this.repositoryCategory = repositoryCategory;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Category> categories = repositoryCategory.GetAll();

            return View(categories);
        }

        // GET - Create
        public IActionResult Create()
        {
            return View();
        }

        // POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)  // проверка модели на валидность
            {
                repositoryCategory.Add(category);
                repositoryCategory.Save();
                return RedirectToAction("Index");  // переход на страницу категорий
            }

            return View(category);
        }

        // GET - Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = repositoryCategory.Find(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)  // проверка модели на валидность
            {
                repositoryCategory.Update(category);  // !!!
                repositoryCategory.Save();
                return RedirectToAction("Index");  // переход на страницу категорий
            }

            return View(category);
        }

        // GET - Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = repositoryCategory.Find(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = repositoryCategory.Find(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            repositoryCategory.Remove(category);
            repositoryCategory.Save();

            return RedirectToAction("Index");
        }
    }
}
