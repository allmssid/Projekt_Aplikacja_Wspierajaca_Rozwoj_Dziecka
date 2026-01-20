using System.Linq;
using System.Threading.Tasks;
using Aplikacja_wspierajaca_rozwoj_dziecka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Profile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new ProfileViewModel
            {
                Email = user.Email ?? string.Empty,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return View(model);
        }

        // POST: /Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // aktualizacja podstawowych danych
            user.Email = model.Email;
            user.UserName = model.Email; // jeśli logujemy się po e-mailu
            user.Imie = model.Imie;
            user.Nazwisko = model.Nazwisko;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            TempData["ProfileUpdated"] = "Dane profilu zostały zaktualizowane.";
            return RedirectToAction(nameof(Index));
        }
    }
}
