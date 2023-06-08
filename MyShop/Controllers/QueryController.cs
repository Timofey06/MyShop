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
using MyShop_Models.Models.ViewModels;
using MyShop_Models.ViewModels;
using MyShop_Utility;
using System.Security.Claims;

namespace MyShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class QueryController:Controller
    {
        IRepositoryQueryDetail repositoryQueryDetail;
        IRepositoryQueryHeader repositoryQueryHeader;
        [BindProperty]
        public  QueryViewModel QueryViewModel { get; set; }
        public QueryController(IRepositoryQueryDetail repositoryQueryDetail, IRepositoryQueryHeader repositoryQueryHeader)
        {
            this.repositoryQueryDetail = repositoryQueryDetail;
            this.repositoryQueryHeader = repositoryQueryHeader;
        }
        public IActionResult Index()
        {
            

            return View();
        }
        public IActionResult Details(int id)
        {
            QueryViewModel queryViewModel = new QueryViewModel()
            {
                //извлекаем хедер из репозитория
                QueryHeader = repositoryQueryHeader.FirstOrDefault(x=>x.Id==id),
                QueryDetails=repositoryQueryDetail.GetAll(x=>x.QueryHeaderId==id,includeProperties: "Product")
            };
            return View(queryViewModel);
        }
        [HttpPost]
        public IActionResult Details()
        {
            List<Cart> carts = new List<Cart>();
            QueryViewModel.QueryDetails = repositoryQueryDetail.GetAll
                (x => x.QueryHeaderId == QueryViewModel.QueryHeader.Id);
            //создаём корзину покупок и добавляем значение в сессию
            foreach (var item in QueryViewModel.QueryDetails)
            {
                Cart cart = new Cart() { ProductId = item.ProductId };
                carts.Add(cart);
            }
            //работа с сессиями
            HttpContext.Session.Clear();
            HttpContext.Session.Set(PathManager.SessionCard, carts);
            //сессия флаг того что мы редактируем заказ
            HttpContext.Session.Set(PathManager.SessionQuery, QueryViewModel.QueryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public IActionResult Delete()
        {
            QueryHeader queryHeader = repositoryQueryHeader.FirstOrDefault
                (x => x.Id == QueryViewModel.QueryHeader.Id);
            IEnumerable<QueryDetail> queryDetails = repositoryQueryDetail.GetAll
                (x => x.QueryHeaderId == queryHeader.Id);
            repositoryQueryDetail.Remove(queryDetails);
            
            repositoryQueryHeader.Remove(queryHeader);
            
            repositoryQueryHeader.Save();

            return RedirectToAction("Index");
            
        }
        public IActionResult GetQueryList()
        {
            JsonResult result = Json(new
            {
                data = repositoryQueryHeader.GetAll()
            });
            return result;
        }
    }
}
