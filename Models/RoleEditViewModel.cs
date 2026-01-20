using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    public class RoleEditViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "E-mail")]
        [ValidateNever]         // <-- NIE waliduj tego pola
        public string? Email { get; set; }

        [Display(Name = "Imię")]
        public string? Imie { get; set; }

        [Display(Name = "Nazwisko")]
        public string? Nazwisko { get; set; }

        [Display(Name = "Aktualna ranga")]
        public string? CurrentRole { get; set; }

        [Display(Name = "Nowa ranga")]
        public string? SelectedRole { get; set; }

        public List<string> AvailableRoles { get; set; } = new();
    }
}
