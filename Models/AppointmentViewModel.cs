using Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class AppointmentViewModel : BaseViewModel
	{
		[Required(ErrorMessage = "Thời gian đặt không được bỏ trống")]
		public DateTime AtTime { get; set; }
		[Required(ErrorMessage = "Bạn phải chọn trạng thái cho lịch đặt")]
		public EAppointmentState State { get; set; }
		[Required(ErrorMessage = "Chọn hồ sơ đặt lịch")]
		public uint? ProfileId { get; set; }
		[Required(ErrorMessage = "Chọn bác sĩ khám")]
		public uint DoctorId { get; set; }
	}
}
