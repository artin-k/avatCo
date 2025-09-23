namespace avatCo.Models.ViewModel
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public decimal Price { get; set; }
        public string Brand { get; set; } = "";
        public string Material { get; set; } = "";
        public bool IsActive { get; set; }
        public string Description { get; set; } = "";
        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Review> Opinions { get; set; } = new();
        public List<Review> Questions { get; set; } = new();
    }
}


