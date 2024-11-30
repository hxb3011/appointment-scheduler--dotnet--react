using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class ProfileModel
    {
        public uint Id { get; set; }
        public uint Patient { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public char Gender { get; set; }
    }
}
