
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MyShop_DataMigrations;
using MyShop_DataMigrations.Repository;
using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models;
using MyShop_Models.Models;
using MyShop_Models.ViewModels;
using MyShop_Utility;
using MyShop_Utility.BrainTree;
using System.Security.Claims;

namespace MyShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        //ApplicationDbContext db;
        ProductUserViewModel productUserViewModel;
        IWebHostEnvironment webHostEnvironment;
        IEmailSender emailSender;
        IRepositoryApplicationUser repositoryApplicationUser;
        IRepositoryProduct repositoryProduct;
        IRepositoryQueryHeader repositoryQueryHeader;
        IRepositoryQueryDetail repositoryQueryDetail;
        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;
        IBrainTreeBridge brainTreeBridge;
        public CartController(/*ApplicationDbContext db*/ IWebHostEnvironment webHostEnvironment, IEmailSender emailSender
, IRepositoryProduct repositoryProduct, IRepositoryApplicationUser repositoryApplicationUser,
            IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail,
            IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail,
            IBrainTreeBridge brainTreeBridge)
        {

            this.webHostEnvironment = webHostEnvironment;
            this.emailSender = emailSender;
            this.repositoryProduct = repositoryProduct;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.brainTreeBridge = brainTreeBridge;
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
            IEnumerable<Product> productListTemp = 
                repositoryProduct.GetAll(x => productIdList.Contains(x.Id));
            List<Product> productList = new List<Product>();
            foreach (var item in cartList)
            {
                Product product = productListTemp.FirstOrDefault(x => x.Id == item.ProductId);
                product.TempCount = item.Count;
                productList.Add(product);
            }
            return View(productList);
        }
        
        public IActionResult InquiryConfirmation(int id=0)
        {
            OrderHeader orderHeader=repositoryOrderHeader.FirstOrDefault(x=>x.Id == id);
            HttpContext.Session.Clear();
            return View(orderHeader);
        }
        [HttpPost]
        
        public async Task<IActionResult> SummaryPost(IFormCollection collection,
            ProductUserViewModel productUserViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
           
            if (User.IsInRole(PathManager.AdminRole))
            {
                double totalPrice = 0;
                foreach (var item in productUserViewModel.ProductList)
                {
                    totalPrice += item.TempCount * item.Price;
                }
                //WorkWithOrder
                OrderHeader orderHeader = new OrderHeader()
                {
                    AdminId = claim.Value,
                    OrderDate=DateTime.UtcNow,
                    TotalPrice=totalPrice,
                    Status=PathManager.StatusPending,
                    FullName=productUserViewModel.ApplicationUser.FullName,
                    PhoneNumber=productUserViewModel.ApplicationUser.PhoneNumber,
                    Email=productUserViewModel.ApplicationUser.Email,
                    City=productUserViewModel.ApplicationUser.City,
                    Street=productUserViewModel.ApplicationUser.Street,
                    House=productUserViewModel.ApplicationUser.House,
                    Apartament=productUserViewModel.ApplicationUser.Apartament,
                    PostalCode=productUserViewModel.ApplicationUser.PostalCode,


                };

                string nonce = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = 1,
                    PaymentMethodNonce = nonce,
                    OrderId = "1",
                    Options = new TransactionOptionsRequest { SubmitForSettlement = true }
                };
                var getWay = brainTreeBridge.GetGateWay();
                var resultTransaction = getWay.Transaction.Sale(request);
                var id = resultTransaction.Target.Id;
                orderHeader.TransactionId = id;
                var status = resultTransaction.Target.ProcessorResponseText;

                repositoryOrderHeader.Add(orderHeader);
                repositoryOrderHeader.Save();
                foreach (var product in productUserViewModel.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderHeaderId=orderHeader.Id,
                        ProductId=product.Id,
                        Count=product.TempCount,
                        PricePerUnit=(int)product.Price
                    };
                    repositoryOrderDetail.Add(orderDetail);

                }
                repositoryOrderDetail.Save();

                


                return RedirectToAction("InquiryConfirmation",new { id=orderHeader.Id } );
            }
            else
            {
                //WorkWithQuery
                var path = webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates"
               + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";
                var subject = "New query";
                string bodyHtml = "";
                using (StreamReader reader = new StreamReader(path))
                {
                    bodyHtml = reader.ReadToEnd();
                }
                string textProducts = "";
                foreach (var item in productUserViewModel.ProductList)
                {
                    textProducts += $"Name: {item.Name}, ID: {item.Id}\n";
                }
                string body = string.Format(bodyHtml,
                    productUserViewModel.ApplicationUser.FullName,
                    productUserViewModel.ApplicationUser.Email,
                    productUserViewModel.ApplicationUser.PhoneNumber,
                    textProducts
                    );
                await emailSender.SendEmailAsync("tima.loktionov6@gmail.com", subject, body);
                await emailSender.SendEmailAsync(productUserViewModel.ApplicationUser.Email, subject, body);

                //add data to db by summary

                QueryHeader queryHeader = new QueryHeader()
                {
                    ApplicationUserId = productUserViewModel.ApplicationUser.Id,
                    ApplicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                    PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
                    FullName = productUserViewModel.ApplicationUser.FullName,
                    QueryDate = DateTime.UtcNow,
                    Email = productUserViewModel.ApplicationUser.Email,
                };
                
                repositoryQueryHeader.Add(queryHeader);

                repositoryQueryHeader.Save();
                foreach (var item in productUserViewModel.ProductList)
                {
                    QueryDetail queryDetail = new QueryDetail()
                    {
                        QueryHeader = queryHeader,
                        QueryHeaderId = queryHeader.Id,
                        ProductId = item.Id,
                        Product = repositoryProduct.FirstOrDefault(x => x.Id == item.Id)

                    };
                    repositoryQueryDetail.Add(queryDetail);

                }
                repositoryQueryDetail.Save();

            }




            return RedirectToAction("InquiryConfirmation");
        }
        [HttpPost]
        public IActionResult Summary()
        {
            ApplicationUser applicationUser;
            if (true)
            {
                if (HttpContext.Session.Get<int>(PathManager.SessionQuery)!=0)
                {
                    QueryHeader queryHeader = repositoryQueryHeader.FirstOrDefault(x => x.Id==HttpContext.Session.Get<int>(PathManager.SessionQuery));
                    applicationUser = new ApplicationUser()
                    {
                        Email = queryHeader.Email,
                        PhoneNumber = queryHeader.PhoneNumber,
                        FullName = queryHeader.FullName
                    };
                }
                else
                {
                    applicationUser = new ApplicationUser();
                }

                var getWay=brainTreeBridge.GetGateWay();
                var tokenClient = getWay.ClientToken.Generate();
                ViewBag.TokenClient=tokenClient;
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                applicationUser = repositoryApplicationUser.FirstOrDefault(x=>x.Id==claim.Value);
            }





           
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard) != null
               && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCard).ToList();
                //хотим получить каждый товар из корзины


            }
            List<int> productIdList = cartList.Select(x => x.ProductId).ToList();
            IList<Product> productList =
                repositoryProduct.GetAll(x => productIdList.Contains(x.Id)).ToList<Product>();


            productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser=applicationUser,
               
                
            
            };

            foreach (var item in cartList)
            {
                Product product = repositoryProduct.FirstOrDefault(x => x.Id == item.ProductId);
                product.TempCount = item.Count;
                productUserViewModel.ProductList.Add(product);
            }






            return View(productUserViewModel);
        }
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(List<Product> products)
        {
            List<Cart> cartList = new List<Cart>();
            foreach (var product in products)
            {
                cartList.Add(new Cart()
                {
                    ProductId = product.Id,
                    Count = product.TempCount
                });
            }
            HttpContext.Session.Set(PathManager.SessionCard, cartList);
            return 
               RedirectToAction("Summary");
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
        [HttpPost]
        public IActionResult Update(List<Product> products)
        {
            List<Cart> cartList = new List<Cart>();
            foreach (Product product in products)
            {
                cartList.Add(new Cart()
                {
                    ProductId=product.Id,
                    Count=product.TempCount
                });
            }
            HttpContext.Session.Set(PathManager.SessionCard, cartList);
            return RedirectToAction("Index");
        }
        
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
