// Namn och beskrivning om de olika frisörer som finns
// Hantering av dessa är ej åtkomligt för kunder, därav ingen required

namespace LUXLocks_projekt.Models {
    public class StylistModel {
        // Properties
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Bio { get; set; }

        public List<AppointmentModel>? Appointments { get; set; }
    }
}