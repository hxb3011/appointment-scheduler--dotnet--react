using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ScheduleInfo(uint id)
        {
            var schedule = await _scheduleService.GetScheduleById(id);
            return Ok(schedule);
        }
    }
}
