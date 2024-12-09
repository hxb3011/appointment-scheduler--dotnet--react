using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly ProfileService _profileService;
        private readonly ScheduleService _scheduleService;

        public AppointmentController(AppointmentService appointmentService, 
            DoctorService doctorService, 
            ProfileService profileService,
            ScheduleService schedule)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _profileService = profileService;
            _scheduleService = schedule;
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

        public async Task<IActionResult> AppointmentInfo(uint id)
        {
            var appointment = await _appointmentService.GetAppointmentById(id);
            return Ok(appointment);
        }

        public async Task<IActionResult> Index(int offset = 0, int count = 1000)
        {

			PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = offset;
            pagedGetAllRequest.Count = count;
            var appointments = await _appointmentService.GetPagedAppointmentsWithBodyAsync(pagedGetAllRequest);
            
            return View(appointments);
        }

        public async Task<IActionResult> Create()
        {
            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);
            ViewBag.Doctors = new SelectList(doctors, "Id", "FullName");

            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest);
            ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");

            var schedules = await _scheduleService.GetAll();
            ViewBag.Schedules = new SelectList(schedules, "Id", "Start");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentModel appointment)
        {
            //var random = new Random();
            //appointment.Number = (uint)random.Next(1, 1000);
            if (appointment.EndTime != default)
            {
                if(appointment.Date != null)
                {
                    // Tạo một DateTime từ EndTime
                    DateTime endDateTime = new DateTime(appointment.Date.Value.Year, appointment.Date.Value.Month, appointment.Date.Value.Day, appointment.EndTime.Hour, appointment.EndTime.Minute, 0);

                    // Trừ 30 phút từ EndTime để tính BeginTime
                    DateTime beginDateTime = endDateTime.AddMinutes(-30);

                    // Cập nhật BeginTime trong AppointmentModel
                    appointment.BeginTime = new TimeOnly(beginDateTime.Hour, beginDateTime.Minute);
                }
                if (await _appointmentService.AddAppointment(appointment))
                {
                    TempData["Success"] = "Thêm mới đặt lịch thành công";
                    return RedirectToAction(nameof(Index));
                }
            }

           

            TempData["Error"] = "Đã có lỗi xả ra";
            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest);
            ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");

            var schedules = await _scheduleService.GetAll();
            ViewBag.Schedules = new SelectList(schedules, "Id", "Start");

            ViewBag.SelectedDoctorId = appointment.Doctor;
            ViewBag.SelectedProfileId = appointment.Profile;
            return View(appointment);
        }



        public async Task<IActionResult> Edit(uint id)
        {
            var appointment = await _appointmentService.GetAppointmentResponseById(id);
            if (appointment == null)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi truy cập đơn đặt này";
                return View("Error");
            }

            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest);
            ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");

            ViewBag.SelectedDoctorId = appointment.Doctor;
            ViewBag.SelectedProfileId = appointment.Profile;

            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentResponseModel appointment)
        {
            if (await _appointmentService.UpdateAppointment(appointment.Id.Value, appointment.Profile.Value))
            {
                TempData["Success"] = "Chỉnh sửa đặt lịch thành công";
                return RedirectToAction("Index");
            }

            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest);
            ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");

            ViewBag.SelectedDoctorId = appointment.Doctor;
            ViewBag.SelectedProfileId = appointment.Profile;
            TempData["Error"] = "Đã có lỗi xảy ra";
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(uint id)
        {
            if (await _appointmentService.DeleteAppointment(id))
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
