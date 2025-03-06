using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LUXLocks_projekt.Models {
    public class StylistModel {
        // Properties
        public int Id { get; set; }

        [Required(ErrorMessage = "Du måste ange namn på personalen.")]
        [Display(Name = "Utövare:")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Du måste ange en biografi.")]
        [Display(Name = "Biografi:")]
        public string? Bio { get; set; }

        // lagra filnamn för bild i databasen
        [Display(Name = "Profilbild:")]
        public string? ImageName { get; set; }

        // visa i gränssnitt
        [NotMapped] // lagra ej i databasen
        [Display(Name = "Profilbild:")]
        public IFormFile? ImageFile { get; set;}

        public List<AppointmentModel>? Appointments { get; set; }
    }
}