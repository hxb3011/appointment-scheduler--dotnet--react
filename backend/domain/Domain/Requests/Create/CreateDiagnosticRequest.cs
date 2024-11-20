using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Domain.Requests.Create
{
    public class CreateDiagnosticRequest
    {
        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên dịch vụ không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Appointment ID là bắt buộc.")]
        public uint AppointmentId { get; set; }

        [StringLength(250, ErrorMessage = "Mô tả không được vượt quá 250 ký tự.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Doctor ID là bắt buộc.")]
        public uint DoctorId { get; set; }
    }
}
