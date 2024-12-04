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
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(doctor, new ValidationContext(doctor), results, true);
            string resultMessage = "Lỗi không thể thêm mới bác sĩ này";
            if (isValid)
            {
                resultMessage = await _doctorService.AddDoctor(doctor);

                if (resultMessage == "Doctor added successfully")
                {
                    TempData["Success"] = "Thêm mới bác sĩ thành công";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Error"] = resultMessage;

            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest
            {
                Offset = 0,
                Count = 20
            };

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

            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = 0;
            pagedGetAllRequest.Count = 20;

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorModel doctor)
        {
            if(await _doctorService.UpdateDoctor(doctor))
            {
                TempData["Success"] = "Chỉnh sửa bác sĩ thành công";
                return RedirectToAction("Index");
            }

            PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
            pagedGetAllRequest.Offset = 0;
            pagedGetAllRequest.Count = 20;

            var roles = await _roleService.GetPagedRoles(pagedGetAllRequest);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            TempData["Error"] = "Đã có lỗi xảy ra";
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
