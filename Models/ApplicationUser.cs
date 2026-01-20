using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Aplikacja_wspierajaca_rozwoj_dziecka.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string? Imie { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string? Nazwisko { get; set; }

        // Tu później możesz dodać np. powiązanie z DzieckoId itd.
    }
}
