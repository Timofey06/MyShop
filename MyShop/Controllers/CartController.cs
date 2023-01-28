using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MyShop_DataMigrations;
using MyShop_Models;
using MyShop_Models.ViewModels;
using MyShop_Utility;
using System.Security.Claims;

namespace MyShop.Controllers
{
    
    public class CartController : Controller
    {
       ApplicationDbContext db;
        ProductUserViewModel productUserViewModel;
        IWebHostEnvironment webHostEnvironment;
        IEmailSender emailSender;
        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
            this.emailSender=emailSender;
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
        
        public IActionResult InquiryConfirmation()
        {

            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {

           var path= webHostEnvironment.WebRootPath+Path.DirectorySeparatorChar.ToString()+"templates"
                +Path.DirectorySeparatorChar.ToString()+"Inquiry.html";

            var subject = "New order";
            string bodyHtml = "";
            using(StreamReader reader=new StreamReader(path))
            {
                bodyHtml = reader.ReadToEnd();
            }
            string textProducts = "";
            foreach (var item in productUserViewModel.ProductList)
            {
                textProducts += $"Name: {item.Name}, ID: {item.Id}\n";
            }
            string body=string.Format(bodyHtml,
                productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.Email,
                productUserViewModel.ApplicationUser.PhoneNumber,
                textProducts
                );
            await emailSender.SendEmailAsync("tima.loktionov6@gmail.com",subject,body);
            await emailSender.SendEmailAsync(productUserViewModel.ApplicationUser.Email, subject, body);
            return RedirectToAction("InquiryConfirmation");
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
            IList<Product> productList =
                db.Product.Where(x => productIdList.Contains(x.Id)).ToList<Product>();


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
