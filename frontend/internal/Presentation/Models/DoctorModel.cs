using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class DoctorModel
    {
        public uint Id { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string Certificate { get; set; }
        public string Password { get; set; }
        [JsonPropertyName("roleId")]
        public uint RoleId { get; set; }
    }
}
