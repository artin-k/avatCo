using System.ComponentModel.DataAnnotations.Schema;

namespace avatCo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public string Brand {  get; set; } = string.Empty;
        public string Material {  get; set; } = string.Empty;
        // Foreign Key
        public int CategoryId { get; set; }
        // Navigation
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = false;
        public bool IsSpecialOffer { get; set; } = false;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
