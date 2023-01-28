using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop_Models;


namespace MyShop.Controllers
{
    //[Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private ApplicationDbContext db;

        public MyModelController(ApplicationDbContext db)
        {
            this.db = db;
        }

        
        public IActionResult Index()
        {
            IEnumerable<MyModel> models = db.MyModel;

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
                db.MyModel.Add(myModel);
                db.SaveChanges();
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

            var myModel = db.MyModel.Find(id);

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
                db.MyModel.Update(myModel); 
                db.SaveChanges();
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

            var myModel = db.MyModel.Find(id);

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
            var myModel = db.MyModel.Find(id);

            if (myModel == null)
            {
                return NotFound();
            }

            db.MyModel.Remove(myModel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

