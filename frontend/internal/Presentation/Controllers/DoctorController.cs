using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly RoleService _roleService;

        public DoctorController(DoctorService doctorService, RoleService roleService)
        {
            _doctorService = doctorService;
            _roleService = roleService;
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

        public async Task<IActionResult> DoctorInfo(uint id)
        {
            var doctor = await _doctorService.GetDoctorById(id);
            return Ok(doctor);
        }

        public async Task<IActionResult> Index(int offset = 0, int count = 1000)
        {
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = offset;
            pagedGetAllRequest.Count = count;
            var doctors = await _doctorService.GetPagedDoctors(pagedGetAllRequest);

            return View(doctors);
        }

        public async Task<IActionResult> Create()
        {
            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = 0;
            pagedGetAllRequest.Count = 20;

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorModel doctor)
        {
            string resultMessage = "Lỗi không thể thêm mới bác sĩ này";
            if (ModelState.IsValid)
            {
                resultMessage = await _doctorService.AddDoctor(doctor);

                if (resultMessage == "Doctor added successfully")
                {
                    TempData["Success"] = "Thêm mới bác sĩ thành công";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Error"] = resultMessage;

            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(doctor);
        }


        public async Task<IActionResult> Edit(uint id)
        {
            var doctor = await _doctorService.GetDoctorById(id);
            if(doctor == null)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi truy cập bác sĩ này";
                return View("Error");
            }

            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorModel doctor)
        {
            string resultMessage = "Lỗi không thể sửa bác sĩ này";
            if (ModelState.IsValid)
            {
                resultMessage = await _doctorService.UpdateDoctor(doctor);
                if (resultMessage == "Doctor updated successfully")
                {
                    TempData["Success"] = "Chỉnh sửa bác sĩ thành công";
                    return RedirectToAction("Index");
                }
            }

            PagedGetAllRequest pagedGetAllRequest = GetPage();

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            TempData["Error"] = resultMessage;
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(uint id)
        {
            if (await _doctorService.DeleteDoctor(id))
            {
                return Ok("Xóa bác sĩ thành công");
            }
            return BadRequest("Không thể xóa bác sĩ");
        }

    }
}
