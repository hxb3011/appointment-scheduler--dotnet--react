using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly ExaminationService _examinationService;
        private readonly AppointmentService _appointmentService;
        private readonly DiagnosticSerService _diagnosticSerService;
        private readonly DoctorService _doctorService;
        private readonly ILogger<ExaminationController> _logger;

        public ExaminationController(ExaminationService examinationService, 
            AppointmentService appointmentService, 
            DiagnosticSerService diagnosticSerService,
            DoctorService doctorService,
            ILogger<ExaminationController> logger)
        {
            _examinationService = examinationService;
            _appointmentService = appointmentService;
            _diagnosticSerService = diagnosticSerService;
            _doctorService = doctorService;
            _logger = logger;
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
            var examinations = await _examinationService.GetPagedExamination(pagedGetAllRequest);

            return View(examinations);
        }

        public async Task<IActionResult> Create()
        {
            var page = getPage();

            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExaminationModel examination)
        {
            string resultMessage = "Lỗi không thể thêm khám bệnh này";
            if (examination.Appointment != null)
            {
                resultMessage = await _examinationService.AddExamination(examination.Appointment.Value);

                if (resultMessage == "Examination added successfully")
                {
                    TempData["Success"] = "Thêm mới khám bệnh thành công";
                    return RedirectToAction(nameof(Index));
                }

            }

            var page = getPage();

            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            TempData["Error"] = resultMessage;
            return View();
        }

        public async Task<IActionResult> Edit(uint id)
        {
            var exam = await _examinationService.GetExaminationById(id);
            if (exam == null)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi truy cập khám bệnh này";
                return View("Error");
            }

            var page = getPage();

            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

			var diagnosticServices = await _diagnosticSerService.GetPagedDiagnosticSers(page);
            ViewBag.Diagnostics = diagnosticServices;

            var doctors = await _doctorService.GetPagedDoctors(page);
            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            ViewBag.SelectedAppointmentIdInExam = exam.Appointment;

            return View(exam);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(ExaminationModel exam)
        //{
        //    string resultMessage = "Lỗi không thể sửa khám bệnh này";
        //    if (ModelState.IsValid)
        //    {
        //        resultMessage = await _examinationService.UpdateExamination(exam);
        //        if (resultMessage == "Examination updated successfully")
        //        {
        //            TempData["Success"] = "Chỉnh sửa khám bệnh thành công";
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    TempData["Error"] = resultMessage;
        //    var page = getPage();

        //    var appointments = await _appointmentService.GetPagedAppointments(page);
        //    ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

        //    var diagnosticServices = await _diagnosticSerService.GetPagedDiagnosticSers(page);
        //    ViewBag.Diagnostics = diagnosticServices;

        //    ViewBag.SelectedAppointmentIdInExam = exam.Appointment;
        //    return View(exam);
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(ExaminationModel exam)
        {
            string resultMessage = "Lỗi không thể sửa khám bệnh này";

            TempData["Error"] = resultMessage;

            var page = getPage();

            // Reload appointments and diagnostics for the form
            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            var diagnosticServices = await _diagnosticSerService.GetPagedDiagnosticSers(page);
            ViewBag.Diagnostics = diagnosticServices;

            var doctors = await _doctorService.GetPagedDoctors(page);
            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            if (exam.SelectedDoctors != null && exam.SelectedDoctors.Count > 0)
            {
                foreach (var doctor in exam.SelectedDoctors)
                {
                    if(doctor.Value != 0)
                    {
                        _logger.LogInformation($"Diagnostic ID: {doctor.Key}, Doctor ID: {doctor.Value}");
                        await _diagnosticSerService.AddExaminationDiagnosticService(doctor.Key, exam.Id, doctor.Value);
                    }
                }
            }

            ViewBag.SelectedAppointmentIdInExam = exam.Appointment;
            return View(exam);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(uint id)
        {
            if (await _examinationService.DeleteExamination(id))
            {
                return Ok("Xóa khám bệnh thành công");
            }
            return BadRequest("Không thể xóa khám bệnh");
        }
    }
}
