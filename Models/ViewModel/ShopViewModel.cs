using System.Collections.Generic;

namespace avatCo.Models.ViewModel
{
    public class ShopViewModel
    {
        public string ShopName { get; set; } = string.Empty;
        public string ShopDescription { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();// ✅ correct type
        public List<Category> Categories { get; set; } = new(); // ✅ correct type


        //
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Material { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public List<Review> Opinions { get; set; } = new();



    }
}
