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
using MyShop_Utility.BrainTree;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Braintree;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class OrderController : Controller
    {
        IRepositoryOrderDetail repositoryOrderDetail;
        IRepositoryOrderHeader repositoryOrderHeader;
        IBrainTreeBridge brainTreeBridge;
        [BindProperty]
        public OrderHeaderDetailViewModel OrderHeaderDetailViewModel { get; set; }
        public OrderController(IRepositoryOrderDetail repositoryOrderDetail, IRepositoryOrderHeader repositoryOrderHeader, IBrainTreeBridge brainTreeBridge)
        {
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.brainTreeBridge = brainTreeBridge;
        }

        public IActionResult Index(string searchName=null,string searchEmail=null, string searchPhone=null, string StatusList=null)
        {
            OrderViewModel viewModel = new OrderViewModel()
            {
                OrderHeaderList = repositoryOrderHeader.GetAll(),
                StatusList = PathManager.StatusList.ToList().
                Select(x => new SelectListItem { Text = x, Value = x })

            };
            if (searchName!=null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.Where(x => x.FullName.ToLower().Contains(searchName.ToLower()));
            }
            if (searchEmail != null)
            {
                viewModel.OrderHeaderList= viewModel.OrderHeaderList.Where(x => x.Email.ToLower().Contains(searchEmail.ToLower()));

            }
            if (searchPhone != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.Where(x => x.PhoneNumber.ToLower().Contains(searchPhone.ToLower()));
            }
            if (StatusList!=null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.Where(x => x.Status == StatusList);
            }
            return View(viewModel);
        }
    
        public IActionResult Details(int id)
        {
            OrderHeaderDetailViewModel = new OrderHeaderDetailViewModel()
            {
                OrderHeader = repositoryOrderHeader.FirstOrDefault(x => x.Id == id),
                
                OrderDetail = repositoryOrderDetail.
                GetAll(x => x.OrderHeaderId == id,includeProperties:"Product")
            };

            return View(OrderHeaderDetailViewModel);
        }
        [HttpPost]
        public IActionResult StartInProcessing()
        {
            // получаем объект из бд
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderHeaderDetailViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusInProcessed;

            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult StartOrderDone()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
               FirstOrDefault(x => x.Id == OrderHeaderDetailViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusOrderDone;

            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
            
        }
        [HttpPost]
        public IActionResult StartOrderCancel()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
               FirstOrDefault(x => x.Id == OrderHeaderDetailViewModel.OrderHeader.Id);

            
            
            if (orderHeader.TransactionId!="")
            {
                var gateWay = brainTreeBridge.GetGateWay();
                Transaction transaction = gateWay.Transaction.Find(orderHeader.TransactionId);
                if (transaction.Status == TransactionStatus.AUTHORIZED ||
                    transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
                {
                    var res = gateWay.Transaction.Void(orderHeader.TransactionId);
                }
                else
                {
                    var res = gateWay.Transaction.Refund(orderHeader.TransactionId);
                }
            }
            
            orderHeader.Status = PathManager.StatusDenied;
            repositoryOrderHeader.Save();
            
            return RedirectToAction("Index");
           
        }
    }
}
