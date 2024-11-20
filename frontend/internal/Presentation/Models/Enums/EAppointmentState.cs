using System.ComponentModel.DataAnnotations;
using AppointmentScheduler.Presentation.Attributes;

namespace AppointmentScheduler.Presentation.Models.Enums
{
	[Metadata("DisplayName", "{}", "Badge", "secondary")]
	public enum EAppointmentState
	{
		[Display(Name = "Không hoạt động")]
		[Metadata("DisplayName", "Không hoạt động", "Badge", "danger")]
		DISABLE = 0,

		[Display(Name = "Hết hạn")]
		[Metadata("DisplayName", "Hết hạn", "Badge", "dark")]
		EXPIRED = 1,

		[Display(Name = "Đang hoạt động")]
		[Metadata("DisplayName", "Đang hoạt động", "Badge", "success")]
		ENABLE = 2
	}
}
