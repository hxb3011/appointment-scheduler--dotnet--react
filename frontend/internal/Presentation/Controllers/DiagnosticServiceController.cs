using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
	public class DiagnosticServiceController : Controller
	{
		private readonly DiagnosticServiceSer _diagnosticService;

        public DiagnosticServiceController(DiagnosticServiceSer diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public async Task<IActionResult> Index()
		{
			var diagnosticServices = await _diagnosticService.GetAllDiagnosticService();

			return View(diagnosticServices);
		}
	}
}
