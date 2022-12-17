namespace MyShop.Models.ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; set; }
        public bool IsInCart { get; set; }
        public DetailsViewModel(Product product, bool isInCart)
        {
            Product = product;
            IsInCart = isInCart;
        }
        public DetailsViewModel()
        {

        }

    }
}
