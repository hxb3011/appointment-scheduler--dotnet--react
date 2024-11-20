using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Presentation.Models;

namespace AppointmentScheduler.Presentation.Services.AppointmentService
{
    public interface IAppointmentService
    {
        public Task<IEnumerable<AppointmentViewModel>> GetAllAppointments();
    }
}
