using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Models.Enums;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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

        public async Task<IActionResult> Create(uint id)
        {
            var page = getPage();

            var examExisted = await _examinationService.ExistExaminationByAppointment(id);

            if(examExisted)
            {
                TempData["Error"] = "Đơn này đã được thêm vào khám bệnh, mời bạn vào khám bệnh để xem";
                return RedirectToAction("Index", "Appointment");
            }

            var appointment = await _appointmentService.GetAppointmentResponseById(id);
			if (appointment.AtTime.HasValue && (DateTime.Now < appointment.AtTime.Value.AddMinutes(-15) || appointment.AtTime.Value.AddMinutes(30) < DateTime.Now))
			{
				TempData["Error"] = "Không trong thời gian khám bệnh, không thể thêm khám bệnh cho đơn này.";
				return RedirectToAction("Index", "Appointment");
			}


			var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            ExaminationModel exam = new ExaminationModel();

            exam.Appointment = id;

            ViewBag.SelectedAppointmentIdInExam = exam.Appointment;

            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExaminationModel examination)
        {
            string resultMessage = "Lỗi không thể thêm khám bệnh này";

            examination.Appointment = examination.Id;

            var appointment = _appointmentService.GetAppointmentById(examination.Appointment.Value);

            if (examination.Appointment != null && appointment != null)
            {
                resultMessage = await _examinationService.AddExamination(examination.Appointment.Value);

                if (resultMessage == "Examination added successfully" && await _appointmentService.ChangeAppointmentStatus(examination.Appointment.Value, (uint)EAppointmentState.ENABLE))
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

            var prescription = await _examinationService.GetPrescription(id);

            if (prescription != null)
            {
                exam.Prescription = prescription.Description;
            }

            var diagnosticServices = await _diagnosticSerService.GetPagedDiagnosticSers(page);
            ViewBag.Diagnostics = diagnosticServices;

            exam.SelectedDiagnostics = new List<uint>();
            exam.SelectedDoctors = new Dictionary<uint, uint>();

            var selectedDoctors = new Dictionary<uint, uint>();

            foreach (var diag in diagnosticServices)
            {
                var diagExam = await _diagnosticSerService.GetExaminationDiagnostic(diag.Id, exam.Id);
                if (diagExam != null)
                {
                    exam.SelectedDiagnostics.Add(diagExam.DiagnosticService);
                    if (diagExam.Doctor != null)
                    {
                        exam.SelectedDoctors.Add(diag.Id, diagExam.Doctor);
                        selectedDoctors[diag.Id] = diagExam.Doctor;
                    }
                }
            }

            HttpContext.Session.SetObjectAsJson("SelectedDoctors", selectedDoctors);

            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            

            var doctors = await _doctorService.GetPagedDoctors(page);
            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            Console.WriteLine(selectedDoctors.Count);
            ViewBag.SelectedAppointmentIdInExam = exam.Appointment;

            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ExaminationModel exam)
        {
            string resultMessage = "Lỗi không thể sửa khám bệnh này";

            if (exam.Diagnostic != "" && exam.Diagnostic != null)
            {
                exam.State = EExaminationState.COMPLETED;
                await _appointmentService.ChangeAppointmentStatus(exam.Appointment.Value, (uint)EAppointmentState.COMPLETED);
            }
            else
            {
                exam.State = EExaminationState.ENABLE;
            }

            var prescription = await _examinationService.GetPrescription(exam.Id);

            if (prescription != null)
            {
                if (exam.Prescription != null)
                {
                    await _examinationService.DeletePerscription(exam.Id);
                    var pres = new PrescriptionRequest();
                    pres.Description = exam.Prescription;
                    await _examinationService.AddPrescription(exam.Id, pres);
                }
                else
                {
                    await _examinationService.DeletePerscription(exam.Id);
                }
            }
            else
            {
                if(exam.Prescription != null)
                {
                    var pres = new PrescriptionRequest();
                    pres.Description = exam.Prescription;
                    await _examinationService.AddPrescription(exam.Id, pres);
                }
            }



            resultMessage = await _examinationService.UpdateExamination(exam);

            var selectedDoctors = HttpContext.Session.GetObjectFromJson<Dictionary<uint, uint>>("SelectedDoctors");

            

            if (resultMessage == "Examination updated successfully")
            {
                TempData["Success"] = "Chỉnh sửa khám bệnh thành công";

                if (exam.SelectedDoctors != null && exam.SelectedDoctors.Count > 0)
                {
                    foreach (var doctor in exam.SelectedDoctors)
                    {
                        if (selectedDoctors.ContainsKey(doctor.Key))
                        {
                            if(doctor.Value != 0)
                            {
                                _logger.LogInformation($"Diagnostic ID: {doctor.Key}, Doctor ID: {doctor.Value}: trung du lieu");
                                await _diagnosticSerService.UpdateExaminationDiagnosticService(doctor.Key, exam.Id, doctor.Value);
                            }
                            else
                            {
                                await _diagnosticSerService.DeleteExaminationDiagnosticService(doctor.Key, exam.Id);
                            }
                        }
                        else if(doctor.Value != 0)
                        {
                            _logger.LogInformation($"Diagnostic ID: {doctor.Key}, Doctor ID: {doctor.Value}");
                            await _diagnosticSerService.AddExaminationDiagnosticService(doctor.Key, exam.Id, doctor.Value);
                        }
                    }
                }

                HttpContext.Session.Remove("SelectedDoctors");

                return RedirectToAction("Index");
            }

            TempData["Error"] = resultMessage;

            var page = getPage();

            // Reload appointments and diagnostics for the form
            var appointments = await _appointmentService.GetPagedAppointments(page);
            ViewBag.Appointments = new SelectList(appointments, "Id", "Id");

            var diagnosticServices = await _diagnosticSerService.GetPagedDiagnosticSers(page);
            ViewBag.Diagnostics = diagnosticServices;

            var doctors = await _doctorService.GetPagedDoctors(page);
            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            ViewBag.SelectedAppointmentIdInExam = exam.Appointment;

            HttpContext.Session.Remove("SelectedDoctors"); // Remove session after processing
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

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }

}
