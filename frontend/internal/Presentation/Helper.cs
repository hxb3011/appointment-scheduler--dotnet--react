using Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppointmentScheduler.Presentation
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
                        case EAppointmentState.ENABLE:
                            return "success";
                        case EAppointmentState.EXPIRED:
                            return "dark";
                    }
                    break;
                default:
                    return "secondary";
            }
            return "secondary";
        }
    }
}
