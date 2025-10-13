namespace avatCo.Models.ViewModel
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalPrice => CartItems.Sum(item => item.Product.Price * item.Quantity);
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }
}
