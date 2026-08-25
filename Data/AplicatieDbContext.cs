using Microsoft.EntityFrameworkCore;
using MyApi.Models;


namespace MyApi.Data
{
    public class AplicatieDbContext : DbContext
    {
        public AplicatieDbContext(DbContextOptions<AplicatieDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produs> Produse { get; set; }
    }
}