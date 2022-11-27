using System;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    public class MyModelController : Controller
    {
        private ApplicationDbContext db;

        public MyModelController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<MyModel> models = db.MyModel;

            return View(models);
        }
    }
}

