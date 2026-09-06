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
        public DbSet<Adresa> Adrese {  get; set; }  
        public DbSet<Comanda> Comenzi {  get; set; }  
        public DbSet<ElementComanda> ElementeComanda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produs>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nume).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Pret).HasPrecision(10, 2);
                entity.Property(p => p.RowVersion).IsRowVersion();

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Produs_Pret_GreaterThanZero", "Pret > 0");
                    t.HasCheckConstraint("CK_Produs_Stoc_GreaterThanOrEqualZero", "Stoc >= 0");
                });

                entity.HasOne(p => p.Categorie)
                      .WithMany(c => c.Produse)
                      .HasForeignKey(p => p.CategorieId);

                
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.IsDeleted).HasDefaultValue(false);
                entity.HasIndex(u => u.Username).IsUnique();
                 
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token).IsRequired().HasMaxLength(250);
                entity.HasIndex(rt => rt.Token).IsUnique();

                entity.HasOne(rt => rt.User)
                      .WithMany(u =>u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);    

            });


            modelBuilder.Entity<Categorie>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Nume).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.Nume).IsUnique();
                entity.Property(c => c.RowVersion).IsRowVersion();

            });


            modelBuilder.Entity<Adresa>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.NumeDestinatar).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Strada).IsRequired().HasMaxLength(150);
                entity.Property(a => a.Oras).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Judet).IsRequired().HasMaxLength(50);
                entity.Property(a => a.CodPostal).IsRequired().HasMaxLength(10); 
                entity.Property(a => a.Tara).IsRequired().HasMaxLength(50);
                entity.Property(a => a.NumarTelefon).IsRequired().HasMaxLength(20);

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Adrese) 
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Comanda>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Total).HasPrecision(18, 2);
                entity.Property(c => c.RowVersion).IsRowVersion();
                entity.Property(c => c.DataCrearii).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);
                entity.ToTable(t => t.HasCheckConstraint("CK_Comanda_Total_GreaterThanOrEqualZero", "Total >= 0"));

                entity.Property(c => c.NumeDestinatar).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Strada).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Oras).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Judet).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Tara).IsRequired().HasMaxLength(50);
                entity.Property(c => c.CodPostal).IsRequired().HasMaxLength(10);
                entity.Property(c => c.TelefonDestinatar).IsRequired().HasMaxLength(20);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comenzi)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

           
            });
            modelBuilder.Entity<ElementComanda>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                entity.Property(ci => ci.PretUnitar).HasPrecision(18, 2);

                entity.ToTable(t => 
                {
                    t.HasCheckConstraint("CK_ElementComanda_Cantitate_GreaterThanZero", "Cantitate > 0");
                    t.HasCheckConstraint("Ck_ElementComanda_PretUnitar_GreaterThanOrEqualZero", "PretUnitar >= 0");
                });
         
                entity.HasOne(ci => ci.Comanda)
                      .WithMany(c => c.ElementeComanda)
                      .HasForeignKey(ci => ci.ComandaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Produs)
                      .WithMany(p => p.ElementeComanda)
                      .HasForeignKey(ci => ci.ProdusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });



        }
    }

}