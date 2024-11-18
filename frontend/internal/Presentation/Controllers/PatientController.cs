using Microsoft.AspNetCore.Mvc;
using Models;
using Services.IService;

namespace AppointmentScheduler.Presentation.Controllers
{
	public class PatientController : Controller
	{
		private readonly IPatientService _patientService;

		public PatientController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		public IActionResult Index()
		{
			var patients = _patientService.GetAllPatients();

			return View(patients);
		}
	}
}
