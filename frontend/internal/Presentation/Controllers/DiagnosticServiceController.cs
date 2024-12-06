using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
	public class DiagnosticServiceController : Controller
	{
		private readonly DiagnosticSerService _diagnosticServiceSer;

        public DiagnosticServiceController(DiagnosticSerService diagnosticService)
        {
            _diagnosticServiceSer = diagnosticService;
        }

        public PagedGetAllRequest getPage()
        {
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest
            {
                Offset = 0,
                Count = 1000
            };

            return pagedGetAllRequest;
        }

        public async Task<IActionResult> Index(int offset = 0, int count = 1000)
        {
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = offset;
            pagedGetAllRequest.Count = count;
            var diagnosticSers = await _diagnosticServiceSer.GetPagedDiagnosticSers(pagedGetAllRequest);

            return View(diagnosticSers);
        }
    }
}
