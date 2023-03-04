using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using MyShop_DataMigrations;
using MyShop_Models;
using MyShop_DataMigrations.Repository.IRepository;

namespace MyShop.Controllers
{
    //[Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private IRepositoryMyModel repositoryMyModel;

        public MyModelController(IRepositoryMyModel repositoryMyModel)
        {
            this.repositoryMyModel = repositoryMyModel;
        }

        
        public IActionResult Index()
        {
            IEnumerable<MyModel> models = repositoryMyModel.GetAll();

            return View(models);
        }
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyModel myModel)
        {
            if (ModelState.IsValid)  
            {
                repositoryMyModel.Add(myModel);
                repositoryMyModel.Save();
                return RedirectToAction("Index");  
            }

            return View(myModel);
        }

        
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var myModel = repositoryMyModel.Find(id.GetValueOrDefault());

            if (myModel == null)
            {
                return NotFound();
            }

            return View(myModel);
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MyModel myModel)
        {
            if (ModelState.IsValid)  
            {
                repositoryMyModel.Update(myModel);
                repositoryMyModel.Save();
                return RedirectToAction("Index");  
            }

            return View(myModel);
        }

        
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var myModel = repositoryMyModel.Find(id.GetValueOrDefault());

            if (myModel == null)
            {
                return NotFound();
            }

            return View(myModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var myModel = repositoryMyModel.Find(id.GetValueOrDefault());

            if (myModel == null)
            {
                return NotFound();
            }

            repositoryMyModel.Remove(myModel);
            repositoryMyModel.Save();

            return RedirectToAction("Index");
        }
    }
}

