using System;
using System.Threading.Tasks;
using Aplikacja_wspierajaca_rozwoj_dziecka.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // UWAGA: zakładamy, że serviceProvider jest już "scoped"
            // (czyli pochodzi z app.Services.CreateScope().ServiceProvider)

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Administrator", "Rodzic", "Dziecko", "Nauczyciel" };

            // Role
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Konto Rodzica
            await CreateUserIfNotExists(
                userManager,
                email: "rodzic@test.pl",
                password: "Rodzic123!",
                imie: "Jan",
                nazwisko: "Rodzic",
                roleName: "Rodzic");

            // Konto Dziecka
            await CreateUserIfNotExists(
                userManager,
                email: "dziecko@test.pl",
                password: "Dziecko123!",
                imie: "Ania",
                nazwisko: "Dzieciak",
                roleName: "Dziecko");

            // Konto Nauczyciela
            await CreateUserIfNotExists(
                userManager,
                email: "nauczyciel@test.pl",
                password: "Nauczyciel123!",
                imie: "Adam",
                nazwisko: "Nauczyciel",
                roleName: "Nauczyciel");

            // Konto Administratora
            await CreateUserIfNotExists(
                userManager,
                email: "admin@test.pl",
                password: "Admin123!",
                imie: "Admin",
                nazwisko: "Systemu",
                roleName: "Administrator");
        }

        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager,
            string email,
            string password,
            string imie,
            string nazwisko,
            string roleName)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return;

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Imie = imie,
                Nazwisko = nazwisko,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
            // w razie błędów można dodać logowanie
        }
    }
}
