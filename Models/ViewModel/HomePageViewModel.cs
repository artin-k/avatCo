namespace avatCo.Models.ViewModels
{
    public class HomePageViewModel
    {
        public string HeroTitle { get; set; } = "Welcome to Avat Co";
        public string HeroSubtitle { get; set; } = "Scalable solutions for modern businesses";
        public List<ServiceItem> Services { get; set; } = new();
        public List<Product> FeaturedProducts { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; }
    }
}