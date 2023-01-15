using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Models.ViewModels;
using MyShop.Utility;
using System.Security.Claims;

namespace MyShop.Controllers
{
    
    public class CartController : Controller
    {
       ApplicationDbContext db;
        ProductUserViewModel productUserViewModel;
        public CartController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
                //хотим получить каждый товар из корзины
            
            
            }
            List<int> productIdList = cartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList = 
                db.Product.Where(x => productIdList.Contains(x.Id));

            return View(productList);
        }
        [HttpPost]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
               && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
                //хотим получить каждый товар из корзины


            }
            List<int> productIdList = cartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList =
                db.Product.Where(x => productIdList.Contains(x.Id));


            productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser=db.ApplicationUser.FirstOrDefault(x=>x.Id==claim.Value),
                ProductList=productList
                
            
            };
            return View(productUserViewModel);
        }
        
        public IActionResult Remove(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
            }
            cartList.Remove(cartList.FirstOrDefault(x=>x.ProductId == id));

            HttpContext.Session.Set(PathManager.SessionCard, cartList);
            return RedirectToAction("Index");
        }
    }
}
