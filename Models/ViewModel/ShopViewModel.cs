using System.Collections.Generic;

namespace avatCo.Models.ViewModel
{
    public class ShopViewModel
    {
        public string ShopName { get; set; } = string.Empty;
        public string ShopDescription { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();// ✅ correct type
        public List<Category> Categories { get; set; } = new(); // ✅ correct type
    }
}
