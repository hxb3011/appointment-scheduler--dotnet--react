using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers;

public class PatientController : Controller
{
	private readonly PatientService _patientService;

	public PatientController(PatientService patientService)
	{
		_patientService = patientService;
	}

	public IActionResult Index()
	{
		var patients = _patientService.GetAllPatients();

		return View(patients);
	}
}
