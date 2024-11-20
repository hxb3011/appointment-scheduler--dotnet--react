using AppointmentScheduler.Presentation.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppointmentScheduler.Presentation.Helper
{
	public static class Helper
	{
		public static string GetDisplayName(this Enum enumValue)
		{
			return enumValue.GetType()
							.GetMember(enumValue.ToString())
							.First()
							.GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
		}

		public static string GetBadge(this Enum enumValue)
		{
			switch (enumValue)
			{
				case EAppointmentState appointmentState:
					switch (appointmentState)
					{
						case EAppointmentState.DISABLE:
							return "danger";
						case EAppointmentState.EXPIRED:
							return "dark";
						case EAppointmentState.ENABLE:
							return "success";
					}
					break;
				default:
					return "secondary";
			}
			return "secondary";
		}
	}
}
