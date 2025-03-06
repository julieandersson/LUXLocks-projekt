using System.ComponentModel.DataAnnotations;

namespace LUXLocks_projekt.Models {
    public class TreatmentModel {
        public int Id { get; set; }

        // tex klippning, färgning
        [Required(ErrorMessage = "Du måste ge behandlingen ett namn")]
        [Display(Name = "Behandling:")]
        public string? Name { get; set; }
        
        // beskrivning av behandling
        [Required(ErrorMessage = "Du måste beskriva behandlingen.")]
        [Display(Name = "Om behandlingen:")]
        public string? Description { get; set; }
        
        // pris på behandling
        [Required(ErrorMessage = "Du måste ange pris på behandlingen.")]
        [Display(Name = "Pris:")]
        public decimal Price { get; set; }

        public List<AppointmentModel>? Appointments { get; set; } // koppling till bokningstabellen
        public List<ReviewModel>? Reviews { get; set; }

    }
}