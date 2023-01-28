using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop_Models;
using MyShop_Models.ViewModels;
using MyShop_Utility;
using System.Diagnostics;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            this.db = db;
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                Products = db.Product,
                Categories=db.Category
            };

            return View(hvm);
        }
        public IActionResult Details(int id)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {

                //Product = db.Product.Find(id),
                Product = db.Product.Include(x => x.Category).Include(x => x.MyModel).Where(x => x.Id == id).FirstOrDefault(),
                IsInCart = false
            };
            foreach (var i in cartList)
            {
                if (i.ProductId==id)
                {
                    detailsViewModel.IsInCart = true;
                }
            }

            return View(detailsViewModel);
        }
        [HttpPost]
        public IActionResult DetailsPost(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard)!=null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count()>0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
            }
            cartList.Add(new Cart() { ProductId = id });

            HttpContext.Session.Set(PathManager.SessionCard, cartList);
            return RedirectToAction("Index");
        }
        
        public IActionResult RemoveFromCart(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
            }
            for (int i = 0; i < cartList.Count; i++)
            {
                if (cartList[i].ProductId==id)
                {
                    cartList.RemoveAt(i);
                    i--;
                }
            }
            HttpContext.Session.Set(PathManager.SessionCard, cartList);
            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}