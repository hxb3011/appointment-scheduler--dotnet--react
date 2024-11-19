using Microsoft.AspNetCore.Mvc;
using Models;
using Services.IService;

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

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AppointmentViewModel appointment)
		{
			if (!ModelState.IsValid)
			{
				return View(appointment);
			}

			if (await _appointmentService.CreateAppointment(appointment))
			{
				TempData["Success"] = "Đặt lịch thành công";
				return RedirectToAction("Index");
			}

			TempData["Error"] = "Đã có lỗi xảy ra";

			return View(appointment);

		}
	}
}
