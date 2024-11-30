using AppointmentScheduler.Domain.Requests;
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

        public async Task<IActionResult> Index(int offset = 0, int count = 1000)
        {
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = offset;
            pagedGetAllRequest.Count = count;
            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);

            return View(doctors);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> DoctorInfo(uint id)
        {
            var doctor = await _doctorService.GetDoctorById(id);
            return Ok(doctor);
        }
    }
}
