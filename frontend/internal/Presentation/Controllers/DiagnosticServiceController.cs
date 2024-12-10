using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduler.Presentation.Controllers
{
	public class DiagnosticServiceController : Controller
	{
		private readonly DiagnosticSerService _diagnosticServiceSer;

        public DiagnosticServiceController(DiagnosticSerService diagnosticService)
        {
            _diagnosticServiceSer = diagnosticService;
        }

        public PagedGetAllRequest GetPage()
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
            PagedGetAllRequest pagedGetAllRequest = GetPage();
            var diagnosticSers = await _diagnosticServiceSer.GetPagedDiagnosticSers(pagedGetAllRequest);

            return View(diagnosticSers);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DiagnosticSerModel diagnostic)
        {
            string resultMessage = "Lỗi không thể thêm hồ sơ này";
            if (ModelState.IsValid)
            {
                resultMessage = await _diagnosticServiceSer.AddDiagnosticService(diagnostic);

                if (resultMessage == "Diagnostic added successfully")
                {
                    TempData["Success"] = "Thêm mới dịch vụ chuẩn đoán thành công";
                    return RedirectToAction(nameof(Index));
                }

            }

            TempData["Error"] = resultMessage;
            return View(diagnostic);
        }

        public async Task<IActionResult> Edit(uint id)
        {
            var diagnostic = await _diagnosticServiceSer.GetDiagnosticSerModelById(id);
            if (diagnostic == null)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi truy cập dịch vụ chuẩn đoán này";
                return View("Error");
            }


            return View(diagnostic);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DiagnosticSerModel diagnostic)
        {
            string resultMessage = "Lỗi không thể sửa dịch vụ chuẩn đoán này";
            if (ModelState.IsValid)
            {
                resultMessage = await _diagnosticServiceSer.UpdateDiagnosticService(diagnostic);
                if (resultMessage == "Diagnostic updated successfully")
                {
                    TempData["Success"] = "Chỉnh sửa dịch vụ chuần đoán thành công";
                    return RedirectToAction("Index");
                }
            }


            TempData["Error"] = resultMessage;
            return View(diagnostic);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(uint id)
        {
            if (await _diagnosticServiceSer.DeleteDiagnosticService(id))
            {
                return Ok("Xóa dịch vụ chuẩn đoán thành công");
            }
            return BadRequest("Không thể xóa dịch vụ chuẩn đoán");
        }
    }
}
