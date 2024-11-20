using AppointmentScheduler.Presentation.Services.AppointmentService;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointments();
            return View(appointments);
        }
    }
}
