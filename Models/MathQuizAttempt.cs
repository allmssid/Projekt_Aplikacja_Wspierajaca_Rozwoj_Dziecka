using System.ComponentModel.DataAnnotations;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    public class MathQuizAttempt
    {
        public int Id { get; set; }

        [Required]
        public string UzytkownikId { get; set; } = null!;

        public ApplicationUser? Uzytkownik { get; set; }

        [Required]
        [Display(Name = "Przedmiot")]
        public QuizSubject Przedmiot { get; set; }

        [Required]
        [Display(Name = "Poziom")]
        public QuizDifficulty Poziom { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data podejścia")]
        public DateTime DataPodejscia { get; set; }

        [Display(Name = "Liczba pytań")]
        public int LiczbaPytan { get; set; }

        [Display(Name = "Poprawne odpowiedzi")]
        public int PoprawneOdpowiedzi { get; set; }
    }
}
