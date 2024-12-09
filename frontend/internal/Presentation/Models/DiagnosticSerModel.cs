using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models
{
    public class DiagnosticSerModel
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Tên dịch vụ chuẩn đoán không được bỏ trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá tiền không được bỏ trống")]
        public double? Price { get; set; }
    }
}
