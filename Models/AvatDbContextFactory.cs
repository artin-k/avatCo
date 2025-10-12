using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace avatCo.Models
{
    public class AvatDbContextFactory : IDesignTimeDbContextFactory<AvatDbContext>
    {
        public AvatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AvatDbContext>();

            // Update to match your current database credentials
            var connectionString = "Server=localhost;Port=3306;Database=avatco;User=appuser;Password=password123;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.39"));

            return new AvatDbContext(optionsBuilder.Options);
        }
    }
}