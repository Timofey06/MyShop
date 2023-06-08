using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyShop_Models.Models.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<OrderHeader> OrderHeaderList { get; set; }
       
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string Status { get; set; }
    }
}
