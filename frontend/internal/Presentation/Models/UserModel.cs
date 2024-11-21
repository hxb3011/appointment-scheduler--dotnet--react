using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models;

public class UserModel : Domain.Entities.User
{
	// Constructor không tham số
	public UserModel() { }
	public UserModel(uint id, string userName, string password, string fullName, char gender, string address)
	{
		Id = id;
		UserName = userName;
		Password = password;
		FullName = fullName;
		Gender = gender;
		Address = address;
	}
	public uint Id { get; set; }
	[Required(ErrorMessage = "UserName không được bỏ trống")]
	public string UserName { get; set; }
	[Required(ErrorMessage = "Password không được bỏ trống")]
	[StringLength(100, MinimumLength = 8, ErrorMessage = "Password phải có độ dài tối thiểu 8 ký tự")]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
		ErrorMessage = "Password phải chứa ít nhất một chữ hoa, một chữ thường, một số và một ký tự đặc biệt")]
	public string Password { get; set; }
	[Required(ErrorMessage = "Họ tên không được bỏ trống")]
	public string FullName { get; set; }
	[Required(ErrorMessage = "Chọn giới tính")]
	[RegularExpression("[MF]", ErrorMessage = "Giới tính không hợp lệ")]
	public char Gender { get; set; }
	// Thuộc tính hiển thị giới tính
	public string GenderDisplay => Gender == 'M' ? "Nam" : "Nữ";
	public string Address { get; set; }
	public uint RoleId { get; set; }
}
