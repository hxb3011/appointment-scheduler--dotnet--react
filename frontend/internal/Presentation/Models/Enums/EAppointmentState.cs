using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models.Enums
{
	public enum EAppointmentState
	{
		[Display(Name = "Không hoạt động")]
		DISABLE = 0,

		[Display(Name = "Hết hạn")]
		EXPIRED = 1,

		[Display(Name = "Đang hoạt động")]
		ENABLE = 2
	}
}
