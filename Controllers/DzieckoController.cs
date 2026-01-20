using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aplikacja_wspierajaca_rozwoj_dziecka.Data;
using Aplikacja_wspierajaca_rozwoj_dziecka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Controllers
{
    [Authorize(Roles = "Administrator,Nauczyciel")]
    public class DzieckoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DzieckoController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dziecko
        public async Task<IActionResult> Index()
        {
            // wszyscy użytkownicy z rolą "Dziecko"
            var usersInChildRole = await _userManager.GetUsersInRoleAsync("Dziecko");
            var childUserIds = usersInChildRole.Select(u => u.Id).ToList();

            // pokazujemy tylko te dzieci, które są powiązane z kontem w roli "Dziecko"
            var dzieci = await _context.Dzieci
                .Include(d => d.User)
                .Where(d => d.UserId != null && childUserIds.Contains(d.UserId))
                .ToListAsync();

            return View(dzieci);
        }

        // GET: Dziecko/SyncFromUsers
        // Tworzy rekordy Dziecko dla wszystkich kont z rolą "Dziecko" bez Dziecko.UserId
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SyncFromUsers()
        {
            var usersInChildRole = await _userManager.GetUsersInRoleAsync("Dziecko");

            var existingChildUserIds = await _context.Dzieci
                .Where(d => d.UserId != null)
                .Select(d => d.UserId!)
                .ToListAsync();

            int createdCount = 0;

            foreach (var user in usersInChildRole)
            {
                if (existingChildUserIds.Contains(user.Id))
                    continue;

                var dziecko = new Dziecko
                {
                    Imie = user.Imie ?? "Imię nieustawione",
                    Nazwisko = user.Nazwisko ?? "",
                    DataUrodzenia = DateTime.Today,
                    Notatki = "Utworzono automatycznie z konta użytkownika.",
                    UserId = user.Id
                };

                _context.Dzieci.Add(dziecko);
                createdCount++;
            }

            if (createdCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            TempData["SyncMessage"] = $"Utworzono {createdCount} brakujących rekordów dzieci.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Dziecko/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dziecko = await _context.Dzieci
                .Include(d => d.UmiejetnosciDziecka)
                    .ThenInclude(du => du.Umiejetnosc)
                .Include(d => d.DziennikiAktywnosci)
                    .ThenInclude(da => da.Aktywnosc)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dziecko == null)
            {
                return NotFound();
            }

            return View(dziecko);
        }

        // GET: Dziecko/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dziecko/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,DataUrodzenia,Notatki")] Dziecko dziecko)
        {
            if (!ModelState.IsValid)
            {
                return View(dziecko);
            }

            // 1. Najpierw zapisujemy dziecko w bazie
            _context.Add(dziecko);
            await _context.SaveChangesAsync();

            // 2. Tworzymy dla niego konto w Identity (na razie techniczny e-mail)
            var email = $"dziecko{dziecko.Id}@app.local";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Imie = dziecko.Imie,
                Nazwisko = dziecko.Nazwisko,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, "Dziecko123!");

            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Dziecko");

                dziecko.UserId = user.Id;
                _context.Update(dziecko);
                await _context.SaveChangesAsync();
            }
            else
            {
                // tu można kiedyś obsłużyć błędy tworzenia konta
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Dziecko/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dziecko = await _context.Dzieci.FindAsync(id);
            if (dziecko == null)
            {
                return NotFound();
            }
            return View(dziecko);
        }

        // POST: Dziecko/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,DataUrodzenia,Notatki")] Dziecko model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dziecko = await _context.Dzieci.FirstOrDefaultAsync(d => d.Id == id);
            if (dziecko == null)
            {
                return NotFound();
            }

            // aktualizujemy tylko pola edytowalne, nie dotykamy UserId
            dziecko.Imie = model.Imie;
            dziecko.Nazwisko = model.Nazwisko;
            dziecko.DataUrodzenia = model.DataUrodzenia;
            dziecko.Notatki = model.Notatki;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DzieckoExists(dziecko.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Dziecko/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dziecko = await _context.Dzieci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dziecko == null)
            {
                return NotFound();
            }

            return View(dziecko);
        }

        // POST: Dziecko/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dziecko = await _context.Dzieci.FindAsync(id);
            if (dziecko != null)
            {
                _context.Dzieci.Remove(dziecko);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DzieckoExists(int id)
        {
            return _context.Dzieci.Any(e => e.Id == id);
        }
    }
}
