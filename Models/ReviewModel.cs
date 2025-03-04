using System.ComponentModel.DataAnnotations;

namespace LUXLocks_projekt.Models {
    public class ReviewModel {
        public int Id { get; set; }

        // vilken behandling som ska recenseras
        [Required(ErrorMessage = "Du måste välja vilken behandling du vill recensera.")]
        public int? TreatmentModelId { get; set; } // koppling till tabell med behandlingar
        public TreatmentModel? Treatment { get; set; }

        // namn på den som skriver recensionen
        [Required(ErrorMessage = "Du måste ange fullständigt namn.")]
        public string? Name { get; set; }

        // betyg 1-5 
        [Required(ErrorMessage = "Betyg krävs.")]
        [Range(1, 5, ErrorMessage = "Betyget måste vara mellan 1 och 5.")]
        public int Rating { get; set; }

        // recension, max 500 tecken
        [StringLength(500, ErrorMessage = "Recensionen får max vara 500 tecken lång.")]
        public string? Comment { get; set; }

        // när recension skapas
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}