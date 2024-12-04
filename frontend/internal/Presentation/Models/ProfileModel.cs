using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class ProfileModel
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Chọn bệnh nhân")]
        public uint? Patient { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Chọn giới tính")]
        public char? Gender { get; set; }
    }
}
