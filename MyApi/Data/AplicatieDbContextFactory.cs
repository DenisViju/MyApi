using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyApi.Data
{
    public class AplicatieDbContextFactory : IDesignTimeDbContextFactory<AplicatieDbContext>
    {
        public AplicatieDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AplicatieDbContext>();
            optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;" +
            "Database=MyApiDb;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True");

            return new AplicatieDbContext(optionsBuilder.Options);
        }
    }
}