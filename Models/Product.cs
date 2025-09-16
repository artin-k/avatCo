namespace avatCo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation
        public Category Category { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
