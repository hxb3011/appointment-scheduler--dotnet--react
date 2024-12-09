using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class PatientModel
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [PaswordValidation]
        public string Password { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone { get; set; }

        public uint RoleId { get; set; }
    }

    
}


