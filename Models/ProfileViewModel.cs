using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Imię")]
        [MaxLength(50)]
        public string? Imie { get; set; }

        [Display(Name = "Nazwisko")]
        [MaxLength(50)]
        public string? Nazwisko { get; set; }

        [Phone]
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Role")]
        public List<string> Roles { get; set; } = new();
    }
}
