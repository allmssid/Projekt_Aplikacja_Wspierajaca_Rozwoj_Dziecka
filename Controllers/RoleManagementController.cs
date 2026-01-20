using Aplikacja_wspierajaca_rozwoj_dziecka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Controllers
{
    [Authorize(Roles = "Administrator,Nauczyciel")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private static readonly string[] AppRoles = new[]
        {
            "Administrator", "Nauczyciel", "Rodzic", "Dziecko"
        };

        public RoleManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Lista użytkowników + ich rangi
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            bool isAdmin = currentRoles.Contains("Administrator");

            var users = _userManager.Users.ToList();
            var model = new List<RoleEditViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var mainRole = roles.FirstOrDefault(r => AppRoles.Contains(r)) ?? "(brak)";

                // nauczyciel nie widzi adminów/nauczycieli
                if (!isAdmin && (roles.Contains("Administrator") || roles.Contains("Nauczyciel")))
                    continue;

                model.Add(new RoleEditViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Imie = user.Imie,
                    Nazwisko = user.Nazwisko,
                    CurrentRole = mainRole
                });
            }

            return View(model);
        }

        // GET: RoleManagement/Edit/userId
        public async Task<IActionResult> Edit(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            bool isAdmin = currentRoles.Contains("Administrator");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (!isAdmin && (roles.Contains("Administrator") || roles.Contains("Nauczyciel")))
                return Forbid();

            var mainRole = roles.FirstOrDefault(r => AppRoles.Contains(r));

            var vm = new RoleEditViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                CurrentRole = mainRole
            };

            if (isAdmin)
            {
                vm.AvailableRoles = AppRoles.ToList();
            }
            else
            {
                vm.AvailableRoles = new List<string> { "Rodzic", "Dziecko" };
            }

            return View(vm);
        }

        // POST: RoleManagement/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            bool isAdmin = currentRoles.Contains("Administrator");

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!isAdmin && (userRoles.Contains("Administrator") || userRoles.Contains("Nauczyciel")))
                return Forbid();

            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                if (isAdmin)
                {
                    if (!AppRoles.Contains(model.SelectedRole))
                        ModelState.AddModelError("", "Nieprawidłowa rola.");
                }
                else
                {
                    if (model.SelectedRole != "Rodzic" && model.SelectedRole != "Dziecko")
                        ModelState.AddModelError("", "Nauczyciel może nadawać tylko role Rodzic/Dziecko.");
                }
            }

            if (!ModelState.IsValid)
            {
                if (isAdmin)
                    model.AvailableRoles = AppRoles.ToList();
                else
                    model.AvailableRoles = new List<string> { "Rodzic", "Dziecko" };

                return View(model);
            }

            // usuwamy wszystkie „nasze” role
            var rolesToRemove = userRoles.Where(r => AppRoles.Contains(r)).ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!string.IsNullOrEmpty(model.SelectedRole))
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
