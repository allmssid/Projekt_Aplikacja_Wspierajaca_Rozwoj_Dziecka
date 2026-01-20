using System.ComponentModel.DataAnnotations;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    public enum QuizSubject
    {
        [Display(Name = "Matematyka")]
        Matematyka = 0,

        [Display(Name = "Język angielski")]
        Angielski = 1
    }

    public enum QuizDifficulty
    {
        [Display(Name = "Poziom podstawowy")]
        Podstawowy = 0,

        [Display(Name = "Poziom średni")]
        Sredni = 1,

        [Display(Name = "Poziom trudny")]
        Trudny = 2
    }
}
