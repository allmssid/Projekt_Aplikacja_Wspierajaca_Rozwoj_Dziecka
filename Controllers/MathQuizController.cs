using Aplikacja_wspierajaca_rozwoj_dziecka.Data;
using Aplikacja_wspierajaca_rozwoj_dziecka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Controllers
{
    [Authorize]
    public class MathQuizController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Random _rnd = new Random();

        // mała „baza słówek”
        private readonly (string Pl, string En, QuizDifficulty Level)[] _englishWords =
        {
            ("kot", "cat", QuizDifficulty.Podstawowy),
            ("pies", "dog", QuizDifficulty.Podstawowy),
            ("dom", "house", QuizDifficulty.Podstawowy),
            ("czerwony", "red", QuizDifficulty.Podstawowy),
            ("książka", "book", QuizDifficulty.Podstawowy),

            ("poniedziałek", "Monday", QuizDifficulty.Sredni),
            ("uczeń", "student", QuizDifficulty.Sredni),
            ("nauczyciel", "teacher", QuizDifficulty.Sredni),
            ("okno", "window", QuizDifficulty.Sredni),
            ("zadanie domowe", "homework", QuizDifficulty.Sredni),

            ("przyjaciel", "friend", QuizDifficulty.Trudny),
            ("wczorajszy", "yesterday's", QuizDifficulty.Trudny),
            ("ważny", "important", QuizDifficulty.Trudny),
            ("tłumaczyć", "explain", QuizDifficulty.Trudny),
            ("rozumieć", "understand", QuizDifficulty.Trudny),
        };

        public MathQuizController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // WYBÓR QUIZU ----------------------------------------------------

        [Authorize(Roles = "Dziecko")]
        public IActionResult Index()
        {
            return View();
        }

        // MATEMATYKA -----------------------------------------------------

        [Authorize(Roles = "Dziecko")]
        public IActionResult StartMath(QuizDifficulty difficulty)
        {
            var model = new MathQuizViewModel { Difficulty = difficulty };

            int questionCount = difficulty switch
            {
                QuizDifficulty.Podstawowy => 5,
                QuizDifficulty.Sredni => 7,
                _ => 10
            };

            int maxNumber = difficulty switch
            {
                QuizDifficulty.Podstawowy => 10,
                QuizDifficulty.Sredni => 20,
                _ => 50
            };

            for (int i = 0; i < questionCount; i++)
            {
                int a = _rnd.Next(1, maxNumber + 1);
                int b = _rnd.Next(1, maxNumber + 1);
                string op = "+";

                if (difficulty == QuizDifficulty.Sredni)
                {
                    op = _rnd.Next(2) == 0 ? "+" : "-";
                }
                else if (difficulty == QuizDifficulty.Trudny)
                {
                    int o = _rnd.Next(3);
                    op = o == 0 ? "+" : o == 1 ? "-" : "*";
                }

                model.Questions.Add(new MathQuizQuestionViewModel
                {
                    A = a,
                    B = b,
                    Operation = op
                });
            }

            return View("Math", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Dziecko")]
        public async Task<IActionResult> StartMath(MathQuizViewModel model)
        {
            if (model.Questions == null || model.Questions.Count == 0)
            {
                ModelState.AddModelError("", "Brak pytań w quizie.");
                return View("Math", model);
            }

            int total = model.Questions.Count;

            int score = model.Questions.Count(q =>
            {
                if (!q.UserAnswer.HasValue) return false;
                int expected = q.Operation switch
                {
                    "-" => q.A - q.B,
                    "*" => q.A * q.B,
                    _ => q.A + q.B
                };
                return q.UserAnswer.Value == expected;
            });

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var attempt = new MathQuizAttempt
            {
                UzytkownikId = userId,
                Przedmiot = QuizSubject.Matematyka,
                Poziom = model.Difficulty,
                DataPodejscia = DateTime.UtcNow,
                LiczbaPytan = total,
                PoprawneOdpowiedzi = score
            };

            _context.MathQuizProby.Add(attempt);
            await _context.SaveChangesAsync();

            return View("Result", attempt);
        }

        // ANGIELSKI ------------------------------------------------------

        [Authorize(Roles = "Dziecko")]
        public IActionResult StartEnglish(QuizDifficulty difficulty)
        {
            var model = new EnglishQuizViewModel { Difficulty = difficulty };

            var available = _englishWords
                .Where(w => w.Level == difficulty)
                .OrderBy(x => _rnd.Next())
                .Take(difficulty switch
                {
                    QuizDifficulty.Podstawowy => 5,
                    QuizDifficulty.Sredni => 7,
                    _ => 10
                })
                .ToList();

            foreach (var w in available)
            {
                model.Questions.Add(new EnglishQuizQuestionViewModel
                {
                    Polish = w.Pl,
                    English = w.En
                });
            }

            return View("English", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Dziecko")]
        public async Task<IActionResult> StartEnglish(EnglishQuizViewModel model)
        {
            if (model.Questions == null || model.Questions.Count == 0)
            {
                ModelState.AddModelError("", "Brak pytań w quizie.");
                return View("English", model);
            }

            int total = model.Questions.Count;

            int score = model.Questions.Count(q =>
                !string.IsNullOrWhiteSpace(q.UserAnswer) &&
                string.Equals(
                    q.UserAnswer.Trim(),
                    q.English.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var attempt = new MathQuizAttempt
            {
                UzytkownikId = userId,
                Przedmiot = QuizSubject.Angielski,
                Poziom = model.Difficulty,
                DataPodejscia = DateTime.UtcNow,
                LiczbaPytan = total,
                PoprawneOdpowiedzi = score
            };

            _context.MathQuizProby.Add(attempt);
            await _context.SaveChangesAsync();

            return View("Result", attempt);
        }

        // MOJE PRÓBY -----------------------------------------------------

        [Authorize(Roles = "Dziecko")]
        public async Task<IActionResult> MyAttempts()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var attempts = await _context.MathQuizProby
                .Where(a => a.UzytkownikId == userId)
                .OrderByDescending(a => a.DataPodejscia)
                .ToListAsync();

            return View(attempts);
        }

        // PODGLĄD DLA RODZICA / NAUCZYCIELA / ADMINA ---------------------

        [Authorize(Roles = "Administrator,Rodzic,Nauczyciel")]
        public async Task<IActionResult> AllAttempts()
        {
            var attempts = await _context.MathQuizProby
                .Include(a => a.Uzytkownik)
                .OrderByDescending(a => a.DataPodejscia)
                .ToListAsync();

            return View(attempts);
        }
    }
}
