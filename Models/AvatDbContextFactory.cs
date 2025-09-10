using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace avatCo.Models
{
    public class AvatDbContextFactory : IDesignTimeDbContextFactory<AvatDbContext>
    {
        public AvatDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AvatDbContext>();

            // Use your real connection string here (or read from config if you want)
            var connectionString = "Server=localhost;Port=3306;Database=avatdb;User=avat;Password=1234;";
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 39)));

            return new AvatDbContext(optionsBuilder.Options);
        }
    }
}
