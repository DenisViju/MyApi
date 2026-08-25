using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MyApi.Data
{
    public class AplicatieDbContextFactory : IDesignTimeDbContextFactory<AplicatieDbContext>
    {
        public AplicatieDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AplicatieDbContext>();
            optionsBuilder.UseSqlite("Data Source=produse.db");

            return new AplicatieDbContext(optionsBuilder.Options);
        }
    }
}