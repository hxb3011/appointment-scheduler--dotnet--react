using AppointmentScheduler.Presentation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Models.Enums
{
	[Metadata("DisplayName", "{}", "Badge", "secondary")]
	public enum EExaminationState
	{
		[Display(Name = "Không hoạt động")]
		[Metadata("DisplayName", "Không hoạt động", "Badge", "danger")]
		DISABLE = 0,

		[Display(Name = "Đang hoạt động")]
		[Metadata("DisplayName", "Đang hoạt động", "Badge", "secondary")]
		ENABLE = 1,

        [Display(Name = "Hoàn thành")]
        [Metadata("DisplayName", "Hoàn thành", "Badge", "success")]
        COMPLETED = 2
    }
}
