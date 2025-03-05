using System.ComponentModel.DataAnnotations;

namespace LUXLocks_projekt.Models {
    public class AppointmentModel {
        // Properties
        public int Id { get; set; }

        // Datum för när behandlingen ska ske
        [Required(ErrorMessage = "Du måste ange datum för din behandling.")]
        [Display(Name = "Datum:")]
        public DateTime? AppointmentDate { get; set; }

        // Namn på den som bokar
        [Required(ErrorMessage = "Du måste ange fullständigt namn.")]
        [Display(Name = "Fullständigt namn:")]
        public string? CustomerName { get; set; }

        // Telefonnummer för den som bokar
        [Required(ErrorMessage = "Du måste ange ett giltigt telefonnummer.")]
        [Phone]
        [Display(Name = "Telefonnummer:")]
        public string? PhoneNumber { get; set; }

        // email för den som bokar
        [Required(ErrorMessage = "Du måste ange en korrekt epostadress.")]
        [EmailAddress]
        [Display(Name = "Epostadress:")]
        public string? Email { get; set; }

        // välj utövare/frisör
        [Required(ErrorMessage = "Välj vem du vill ska utföra din behandling.")]
        [Display(Name = "Välj utövare:")]
        public int? StylistModelId { get; set; } // koppling till frisörtabellen
        public StylistModel? Stylist { get; set; }

        // välj behandling
        [Required(ErrorMessage = "Välj vad för behandling du vill göra.")]
        [Display(Name = "Vad vill du göra?")]
        public int? TreatmentModelId { get; set; } // koppling till tabell med behandlingar
        public TreatmentModel? Treatment { get; set; }

        // ange hårlängd, hårtyp och eventuell annan info
        [Required(ErrorMessage = "Du måste ange din hårlängd.")]
        [Display(Name = "Hårlängd")]
        public string? HairLength { get; set; }

        [Required(ErrorMessage = "Du måste ange din hårtyp.")]
        [Display(Name = "Hårtyp")]
        public string? HairType { get; set; }

        [Display(Name = "Annat du tycker vi bör veta:")]
        public string? AdditionalInfo { get; set; }

        // om man vill ha tyst behandling eller inte, nej som default
        [Display(Name = "Tyst behandling")]
        public bool SilentTreatment { get; set; } = false;
    }
}