using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace avatCo.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
