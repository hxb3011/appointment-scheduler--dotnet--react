using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Controllers;

public class PatientController : Controller
{
	private readonly PatientService _patientService;

	public PatientController(PatientService patientService)
	{
		_patientService = patientService;
	}

    public async Task<IActionResult> PatientInfo(uint id)
    {
        var patient = await _patientService.GetPatientById(id);
        return Ok(patient);
    }

	public async Task<IActionResult> Index(int offset = 0, int count = 1000)
	{
        PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest();
        pagedGetAllRequest.Offset = offset;
        pagedGetAllRequest.Count = count;
        var patients = await _patientService.GetPagedPatients(pagedGetAllRequest);

		return View(patients);
	}

	public async Task<IActionResult> Create()
	{
		return View();
	}

    [HttpPost]
    public async Task<IActionResult> Create(PatientModel patient)
    {
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(patient, new ValidationContext(patient), results, true);
        string resultMessage = "Lỗi không thể thêm bệnh nhân này";
        if (isValid)
        {
            resultMessage = await _patientService.AddPatient(patient);

            if (resultMessage == "Patient added successfully")
            {
                TempData["Success"] = "Thêm mới bệnh nhân thành công";
                return RedirectToAction(nameof(Index));
            }
            
        }
        TempData["Error"] = resultMessage;
        return View(patient);
    }

    public async Task<IActionResult> Edit(uint id)
    {
        var patient = await _patientService.GetPatientById(id);
        if(patient == null)
        {
            TempData["Error"] = "Đã xảy ra lỗi khi truy cập bệnh nhan này";
            return View("Error");
        }

        return View(patient);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PatientModel patient)
    {
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(patient, new ValidationContext(patient), results, true);
        string resultMessage = "Lỗi không thể sửa bệnh nhân này";
        if (isValid)
        {
            resultMessage = await _patientService.UpdatePatient(patient);
            if (resultMessage == "Patient updated successfully")
            {
                TempData["Success"] = "Chỉnh sửa bệnh nhân thành công";
                return RedirectToAction("Index");
            }
        }
        

        TempData["Error"] = resultMessage;
        return View(patient);
    }

	[HttpPost]
	public async Task<IActionResult> Delete(uint id)
	{
		if (await _patientService.DeletePatient(id))
		{
			return Ok("Xóa bệnh nhân thành công");
		}
		return BadRequest("Không thể xóa bệnh nhân");
	}
}
