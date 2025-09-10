namespace avatCo.Models
{
    public class CartItem
    {
        public int Id { get; set; }  // ✅ Primary key

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
