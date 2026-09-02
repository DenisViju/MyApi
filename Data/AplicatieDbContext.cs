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
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

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
               .ToTable(t => t.HasCheckConstraint("CK_Produs_Pret_GreaterThanZero", "Pret >= 0"));

            modelBuilder.Entity<Produs>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Produse)
                .HasForeignKey(p => p.CategorieId);

            modelBuilder.Entity<Produs>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
           


            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);


            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);

            modelBuilder.Entity<RefreshToken>()
               .HasOne(rt => rt.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();



            modelBuilder.Entity<Categorie>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Categorie>()
                .Property(c => c.Nume)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Categorie>()
                .HasIndex(c => c.Nume)
                .IsUnique();

            modelBuilder.Entity<Categorie>()
                .Property(c => c.RowVersion)
                .IsRowVersion();
        }
    }

}