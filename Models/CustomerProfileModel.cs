using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LUXLocks_projekt.Models {
    public class CustomerProfileModel {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; } // Koppling till IdentityUser
        public IdentityUser? User { get; set; }

        // Kort, medellångt, långt
        [Required(ErrorMessage = "Ange din hårlängd.")]
        public string? HairLength { get; set; }

        // Tjockt, tunnt, lockigt, rakt etc
        [Required(ErrorMessage = "Ange din hårtyp.")]
        public string? HairType { get; set; }

        // eventuell extra info eller preferenser
        public string? AdditionalInfo { get; set; }
    }
}
