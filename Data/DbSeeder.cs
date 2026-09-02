using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync
            (AplicatieDbContext context, IPasswordService passwordService, IConfiguration configuration)
        {
            if (!await context.Categorii.AnyAsync())
            {

                    var categorii = new List<Categorie>
                {
                    new Categorie { Nume = "Electronice" },
                    new Categorie { Nume = "Alimentare" },
                    new Categorie { Nume = "Imbracaminte" },
                    new Categorie { Nume = "Casa si gradina" },
                    new Categorie { Nume = "Carti" },
                    new Categorie { Nume = "Sport" }
                };

                    await context.Categorii.AddRangeAsync(categorii);
                    await context.SaveChangesAsync();

                    var produse = new List<Produs>
                {
                    // Electronice
                    new Produs { Nume = "Laptop", Pret = 3500, CategorieId = categorii[0].Id },
                    new Produs { Nume = "Telefon", Pret = 2200, CategorieId = categorii[0].Id },
                    new Produs { Nume = "Casti wireless", Pret = 250, CategorieId = categorii[0].Id },
                    new Produs { Nume = "Mouse", Pret = 80, CategorieId = categorii[0].Id },
                    new Produs { Nume = "Tastatura mecanica", Pret = 320, CategorieId = categorii[0].Id },
                    new Produs { Nume = "Monitor 27 inch", Pret = 1100, CategorieId = categorii[0].Id },

                    // Alimentare
                    new Produs { Nume = "Paine", Pret = 5, CategorieId = categorii[1].Id },
                    new Produs { Nume = "Lapte", Pret = 7.5, CategorieId = categorii[1].Id },
                    new Produs { Nume = "Cafea boabe 1kg", Pret = 55, CategorieId = categorii[1].Id },
                    new Produs { Nume = "Ciocolata", Pret = 12, CategorieId = categorii[1].Id },
                    new Produs { Nume = "Ulei de masline", Pret = 35, CategorieId = categorii[1].Id },

                    // Imbracaminte
                    new Produs { Nume = "Tricou", Pret = 60, CategorieId = categorii[2].Id },
                    new Produs { Nume = "Blugi", Pret = 180, CategorieId = categorii[2].Id },
                    new Produs { Nume = "Geaca de iarna", Pret = 450, CategorieId = categorii[2].Id },
                    new Produs { Nume = "Sepci", Pret = 45, CategorieId = categorii[2].Id },

                    // Casa si gradina
                    new Produs { Nume = "Set unelte gradina", Pret = 220, CategorieId = categorii[3].Id },
                    new Produs { Nume = "Lampa de birou", Pret = 90, CategorieId = categorii[3].Id },
                    new Produs { Nume = "Covor living", Pret = 380, CategorieId = categorii[3].Id },

                    // Carti
                    new Produs { Nume = "Roman istoric", Pret = 45, CategorieId = categorii[4].Id },
                    new Produs { Nume = "Carte de programare", Pret = 120, CategorieId = categorii[4].Id },
                    new Produs { Nume = "Atlas geografic", Pret = 95, CategorieId = categorii[4].Id },

                    // Sport
                    new Produs { Nume = "Minge fotbal", Pret = 75, CategorieId = categorii[5].Id },
                    new Produs { Nume = "Saltea yoga", Pret = 130, CategorieId = categorii[5].Id },
                    new Produs { Nume = "Gantere reglabile", Pret = 300, CategorieId = categorii[5].Id }
                };

                    await context.Produse.AddRangeAsync(produse);
                    await context.SaveChangesAsync();
            }

            var adminUsername = configuration["Admin:Username"];
            var adminPassword = configuration["Admin:Password"];

            if (string.IsNullOrWhiteSpace(adminUsername))
            {
                throw new InvalidOperationException(
                    "Lipseste configuratia Admin:Username din User Secrets.");
            }

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Lipseste configuratia Admin:Password din User Secrets.");
            }

            var admin = await context.Users
                .FirstOrDefaultAsync(u => u.Username == adminUsername);

            bool modificari = false;

            if (admin == null)
            {
                var passwordHash = passwordService.HashPassword(adminPassword);

                admin = new User
                {
                    Username = adminUsername,
                    PasswordHash = passwordHash,
                    Role = "Admin"
                };

                context.Users.Add(admin);

                modificari = true;
            }
            else
            {
                if (admin.Role != "Admin")
                {
                    admin.Role = "Admin";
                    modificari = true;
                }

                if (!passwordService.VerifyPassword(
                        adminPassword,
                        admin.PasswordHash))
                {
                    admin.PasswordHash = passwordService.HashPassword(adminPassword);
                    modificari = true;
                }
            }

            if (modificari)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}