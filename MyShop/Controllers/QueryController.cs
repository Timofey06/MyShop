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
