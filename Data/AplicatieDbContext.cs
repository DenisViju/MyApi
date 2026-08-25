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
        public DbSet<Categorie> Categorii { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produs>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Produs>()
                .Property(p => p.Nume)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Produs>()
                .Property(p => p.Pret)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Produs>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Produse)
                .HasForeignKey(p => p.CategorieId);
        }
    }

}