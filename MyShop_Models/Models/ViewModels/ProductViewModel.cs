using Microsoft.AspNetCore.Mvc.Rendering;
using System;


namespace MyShop_Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }
        public IEnumerable<SelectListItem> MyModelList { get; set; }
    }
}
