using AppointmentScheduler.Presentation.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class AppointmentResponseModel
    {

        [JsonPropertyName("id")]
        public uint? Id { get; set; }

        [JsonPropertyName("at")]
        public DateTime? AtTime { get; set; }

        [JsonPropertyName("profile")]
        public uint? Profile { get; set; } = null;
        [JsonPropertyName("doctor")]
        public uint? Doctor { get; set; } = null;
        [JsonPropertyName("state")]
        public EAppointmentState State { get; set; }

        [JsonPropertyName("number")]
        public uint? Number { get; set; }
    }
}
