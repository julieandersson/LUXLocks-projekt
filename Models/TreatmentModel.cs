// Namn, beskrivning och pris om de olika behandlingar som finns
// Hantering av dessa är ej åtkomligt för kunder, därav ingen required

namespace LUXLocks_projekt.Models {
    public class TreatmentModel {
        public int Id { get; set; }

        // tex klippning, färgning
        public string? Name { get; set; }
        
        // beskrivning av behandling
        public string? Description { get; set; }
        
        // pris på behandling
        public decimal Price { get; set; }

        public List<AppointmentModel>? Appointments { get; set; } // koppling till bokningstabellen
        public List<ReviewModel>? Reviews { get; set; }

    }
}