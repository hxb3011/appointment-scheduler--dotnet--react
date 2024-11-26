using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllDoctors();

            return View(doctors);
        }

        public async Task<IActionResult> DoctorInfo(uint id)
        {
            var doctor = await _doctorService.GetDoctorById(id);
            return Ok(doctor);
        }
    }
}
