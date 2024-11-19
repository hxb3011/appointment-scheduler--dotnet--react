using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IAppointmentService
	{
		Task<IEnumerable<AppointmentViewModel>> GetAllAppointments();
		Task<bool> CreateAppointment(AppointmentViewModel model);
	}
}
