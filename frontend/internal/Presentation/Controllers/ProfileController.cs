using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileService _profileService;
		private readonly PatientService _patientService;

        public ProfileController(ProfileService profileService, PatientService patientService)
        {
            _profileService = profileService;
            _patientService = patientService;
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
            var profiles = await _profileService.GetPagedProfiles(pagedGetAllRequest);

            return View(profiles);
        }

        public async Task<IActionResult> ProfileInfo(uint id)
        {
            var profile = await _profileService.GetProfileById(id);
            return Ok(profile);
        }

		public async Task<IActionResult> Create()
		{
			var page = getPage();

            var patients = await _patientService.GetPagedPatients(page);
			ViewBag.Patients = new SelectList(patients, "Id", "FullName");

            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProfileModel profile)
		{
			string resultMessage = "Lỗi không thể thêm hồ sơ này";
			if (ModelState.IsValid)
			{
                if (profile.DateOfBirth.HasValue)
                {
                    profile.DateOfBirth = DateOnly.FromDateTime(profile.DateOfBirth.Value.ToDateTime(new TimeOnly(0, 0)));
                }
				resultMessage = await _profileService.AddProfile(profile);

				if (resultMessage == "Profile added successfully")
				{
					TempData["Success"] = "Thêm mới hồ sơ thành công";
					return RedirectToAction(nameof(Index));
				}

			}

			var page = getPage();

            var patients = await _patientService.GetPagedPatients(page);
            ViewBag.Patients = new SelectList(patients, "Id", "FullName");

            TempData["Error"] = resultMessage;
			return View(profile);
		}

		public async Task<IActionResult> Edit(uint id)
		{
            var profile = await _profileService.GetProfileById(id);
			if (profile == null)
			{
				TempData["Error"] = "Đã xảy ra lỗi khi truy cập hồ sơ này";
				return View("Error");
			}

            var page = getPage();

            var patients = await _patientService.GetPagedPatients(page);
            ViewBag.Patients = new SelectList(patients, "Id", "FullName");

			ViewBag.SelectedPatientId = profile.Patient;

            return View(profile);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ProfileModel profile)
		{
			string resultMessage = "Lỗi không thể sửa bệnh nhân này";
			if (ModelState.IsValid)
			{
				resultMessage = await _profileService.UpdateProfile(profile);
				if (resultMessage == "Profile updated successfully")
				{
					TempData["Success"] = "Chỉnh sửa bệnh nhân thành công";
					return RedirectToAction("Index");
				}
			}


			TempData["Error"] = resultMessage;
            var page = getPage();

            var patients = await _patientService.GetPagedPatients(page);
            ViewBag.Patients = new SelectList(patients, "Id", "FullName");
            return View(profile);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(uint id)
		{
			if (await _profileService.DeleteProfile(id))
			{
				return Ok("Xóa hồ sơ thành công");
			}
			return BadRequest("Không thể xóa hồ sơ");
		}
	}
}
