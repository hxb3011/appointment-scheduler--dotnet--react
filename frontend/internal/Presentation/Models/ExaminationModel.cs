using AppointmentScheduler.Presentation.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models
{
    public class ExaminationModel
    {
        public uint Id { get; set; }
        public uint? Appointment { get; set; } = null;
        [Required(ErrorMessage = "Chuẩn đoán không được bỏ trống")]
        public string Diagnostic { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Chọn trạng thái")]
        public EExaminationState State { get; set; }

        public List<uint> SelectedDiagnostics { get; set; } = new List<uint>();

        // Ensure this is a dictionary for storing selected diagnostics and their corresponding doctor IDs
        public Dictionary<uint, uint> SelectedDoctors { get; set; } = new Dictionary<uint, uint>();
    }

}
