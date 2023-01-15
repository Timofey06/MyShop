﻿namespace MyShop.Models.ViewModels
{
    public class ProductUserViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        public ProductUserViewModel()
        {
            ProductList = new List<Product>();
        }
    }
}
