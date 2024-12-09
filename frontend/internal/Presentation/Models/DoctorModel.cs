using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.Presentation.Models
{
    public class DoctorModel
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Tên đăng nhập không được chứa khoảng cách")]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string Email { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số")]
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vị trí công việc không được bỏ trống")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Chứng chỉ không được bỏ trống")]
        public string Certificate { get; set; }

        [PaswordValidation]
        public string Password { get; set; }

        [JsonPropertyName("roleId")]
        public uint RoleId { get; set; }
    }

    public class PaswordValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            string password = value.ToString();

            if (password.Length < 6)
            {
                return false;
            }

            bool hasUpperChar = false;
            bool hasLowerChar = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char ch in password)
            {
                if(char.IsUpper(ch)) hasUpperChar = true;
                else if(char.IsLower(ch)) hasLowerChar = true;
                else if(char.IsDigit(ch)) hasDigit = true;
                else if(!char.IsLetterOrDigit(ch)) hasSpecialChar = true;
            }

            return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Mật khẩu phải có ít nhất 6 ký tự, ít nhất 1 số, 1 chữ cái in hoa, 1 chữ cái thường và 1 ký tự đặc biệt";
        }
    }
}
