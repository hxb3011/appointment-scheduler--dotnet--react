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
        private string _token;

        public AppointmentController(AppointmentService appointmentService, DoctorService doctorService, ProfileService profileService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _profileService = profileService;
        }

        public void GetToken()
        {
            _token = HttpContext.Session.GetString("AuthToken");
        }

        public async Task<IActionResult> Index()
        {
            GetToken();

			PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = 0;
            pagedGetAllRequest.Count = 5;
            var appointments = await _appointmentService.GetPagedAppointmentsWithBodyAsync(pagedGetAllRequest, _token);
            
            return View(appointments);
        }

        public async Task<IActionResult> Create()
        {
            GetToken();
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = 0;
            pagedGetAllRequest.Count = 20;

            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest, _token);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest, _token);
            ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(AppointmentModel appointment)
        //{
        //    var random = new Random();
        //    appointment.Number = (uint)random.Next(1, 1000);
        //    if(await _appointmentService.AddAppointment(appointment))
        //    {
        //        TempData["Success"] = "Thêm mới đặt lịch thành công";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    TempData["Error"] = "Đã có lỗi xả ra";
        //    var doctors = await _doctorService.GetAllDoctors();
        //    ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

        //    var profiles = await _profileService.GetAllProfiles();
        //    ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");
        //    return View(appointment);
        //}



        //public async Task<IActionResult> Edit(uint id)
        //{
        //    var appointment = await _appointmentService.GetAppointmentById(id);
        //    if (appointment == null)
        //    {
        //        TempData["Error"] = "Đã xảy ra lỗi khi truy cập đơn đặt này";
        //        return View("Error");
        //    }

        //    var doctors = await _doctorService.GetAllDoctors();
        //    ViewBag.Doctors = new SelectList(doctors, "Id", "Position");

        //    var profiles = await _profileService.GetAllProfiles();
        //    ViewBag.Profiles = new SelectList(profiles, "Id", "FullName");

        //    ViewBag.SelectedDoctorId = appointment.DoctorId;
        //    ViewBag.SelectedProfileId = appointment.ProfileId;

        //    return View(appointment);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(AppointmentModel appointment)
        //{
        //    if (await _appointmentService.UpdateAppointment(appointment))
        //    {
        //        TempData["Success"] = "Chỉnh sửa đặt lịch thành công";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["Error"] = "Đã có lỗi xảy ra";
        //    return View(appointment);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(uint id)
        //{
        //    if(await _appointmentService.DeleteAppointment(id))
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

    }
}
