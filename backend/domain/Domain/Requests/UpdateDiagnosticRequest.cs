using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Domain.Requests
{
    public class UpdateDiagnosticRequest
    {
        [StringLength(100, ErrorMessage = "Tên dịch vụ không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public double? Price { get; set; }

        // Thêm các trường khác nếu cần
    }
}
