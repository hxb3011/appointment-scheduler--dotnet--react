using AppointmentScheduler.Presentation.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models
{
	public class ExaminationModel
	{
		public uint Id { get; set; }
		[Required(ErrorMessage = "Chọn lịch đặt")]
		public uint? Appointment { get; set; }
		[Required(ErrorMessage = "Chuẩn đoán không được bỏ trống")]
		public string Diagnostic { get; set; }
		public string Description { get; set; }
		[Required(ErrorMessage = "Chọn trạng thái")]
		public EExaminationState State { get; set; }
	}
}
