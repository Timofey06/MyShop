using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Utility;

namespace MyShop.Controllers
{
    public class CartController : Controller
    {
       ApplicationDbContext db;
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
        public IActionResult Remove(int? id)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
                


            }
            
            return RedirectToAction("Index");
        }
    }
}
