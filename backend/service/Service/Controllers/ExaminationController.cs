using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExaminationController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<ExaminationController> _logger;

    public ExaminationController(IRepository repository, ILogger<ExaminationController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private ExaminationResponse MakeResponse(IExamination examination)
        => !_repository.TryGetKeyOf(examination, out Examination key) ? null
        : new() { Id = key.Id, Appointment = key.AppointmentId, Diagnostic = key.Diagnostic, Description = key.Description, State = key.State };

    [HttpGet]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadExamination])]
    public ActionResult<IEnumerable<ExaminationResponse>> GetPagedExaminations([FromQuery] PagedGetAllRequest request)
        => Ok(_repository.GetEntities<IExamination>(request.Offset, request.Count, request.By).Select(MakeResponse));

    [HttpGet("byDoctor")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadExamination])]
    public async Task<ActionResult<IEnumerable<ExaminationResponse>>> GetPagedExaminationsByDoctor([FromQuery] PagedGetAllRequest request, uint doctor)
    {
        var d = await _repository.GetEntityBy<uint, IDoctor>(doctor);
        if (d == null) return NotFound("doctor not found");
        return Ok(d.Examinations.AsQueryable().OrderByPropertyName(request.By)
            .Skip(request.Offset).Take(request.Count).Select(MakeResponse));
    }

    [HttpGet("appointment/{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadExamination])]
    public async Task<ActionResult> ExistExaminationByAppointment(uint id)
    {
        var appointment = await _repository.GetEntityBy<uint, IAppointment>(id);
        if (appointment == null) return NotFound();

        var examination = appointment.Examination;

        if(examination == null) return NotFound();

        return Ok();
    }

    [HttpGet("filter")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetPagedExaminationsByProfileAndFilterDates([FromQuery] PagedGetAllRequest request, uint profile, DateOnly start, DateOnly end)
    {
        var p = await _repository.GetEntityBy<uint, IProfile>(profile);
        if (p == null) return NotFound("profile not found");
        return Ok(p.Appointments.AsQueryable().Where(x => DateOnly.FromDateTime(x.AtTime) >= start && DateOnly.FromDateTime(x.AtTime) <= end)
            .OrderByPropertyName(request.By).Skip(request.Offset).Take(request.Count).Select(x => MakeResponse(x.Examination)));
    }

    [HttpGet("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadExamination])]
    public async Task<ActionResult<ExaminationResponse>> GetExamination(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound();
        return Ok(MakeResponse(examination));
    }

    [HttpPost("{appointment}")]
    [JSONWebToken(RequiredPermissions = [Permission.CreateExamination])]
    public async Task<ActionResult> CreateExamination(uint appointment)
    {
        var ap = await _repository.GetEntityBy<uint, IAppointment>(appointment);
        if (ap == null) return BadRequest("can not create");
        var examination = await ap.ObtainExamination();
        if (!await examination.Create())
            return BadRequest("can not create");
        return Ok("success");
    }

    [HttpPut("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.UpdateExamination])]
    public async Task<ActionResult> UpdateExamination([FromBody] UpdateExaminationRequest request, uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound();
        string v;
        if ((v = request.Diagnostic) != null)
            examination.Diagnostic = v;
        if ((v = request.Description) != null)
            examination.Description = v;
        examination.State = request.State;
        if (!await examination.Update())
            return BadRequest("can not update");
        return Ok("success");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.DeleteExamination])]
    public async Task<ActionResult> DeleteExamination(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound();
        if (!await examination.Delete())
            return BadRequest("can not delete");
        return Ok("success");
    }

    [HttpPost("{id}/perscription")]
    [JSONWebToken(RequiredPermissions = [Permission.CreatePrescription])]
    public async Task<ActionResult> CreatePerscription(uint id, [FromBody] PrescriptionRequest request)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return BadRequest("can not create");
        var perscription = await examination.ObtainPrescription();
        perscription.Description = request.Description;
        if (!await perscription.Create())
            return BadRequest("can not create");
        return Ok("success");
    }

    [HttpPost("{id}/perscription/document")]
    [JSONWebToken(RequiredPermissions = [Permission.SealPrescription])]
    public async Task<ActionResult> UploadPerscriptionDocument(uint id, IFormFile document)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound("examination not found");
        var perscription = examination.Prescription;
        if (perscription != null) return NotFound();
        if (document == null || document.Length == 0) return BadRequest("document empty");
        await document.CopyToAsync(perscription.Document(readOnly: false));
        return Ok("success");
    }

    [HttpGet("{id}/perscription")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadPrescription])]
    public async Task<ActionResult<PrescriptionResponse>> GetPerscription(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound("examination not found");
        var perscription = examination.Prescription;
        if (perscription == null) return NotFound();

        return Ok(!_repository.TryGetKeyOf(perscription, out Prescription key) ? null
            : new PrescriptionResponse() { Examination = key.ExaminationId, Description = key.Description });
    }

    [HttpGet("{id}/perscription/document")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadPrescription])]
    public async Task<ActionResult> DownloadPerscriptionDocument(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound("examination not found");
        var perscription = examination.Prescription;
        if (perscription != null) return NotFound();
		var image = perscription.Document(readOnly: true);
		if (image is MemoryStream) return NotFound();
		return File(image, "application/octet-stream");
    }

    [HttpDelete("{id}/perscription")]
    [JSONWebToken(RequiredPermissions = [Permission.DeletePrescription])]
    public async Task<ActionResult> DeletePerscription(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);
        if (examination == null) return NotFound("examination not found");
        var perscription = examination.Prescription;
        if (perscription == null) return NotFound();
        if (!await perscription.Delete())
            return BadRequest("can not delete");
        return Ok("success");
    }

    // [HttpGet("{id}")]
    // public async Task<ActionResult> GetExaminationById(uint id)
    // {
    //     var examination = await _repository.GetEntityBy<uint, IExamination>(id);

    //     if (examination != null)
    //     {
    //         return Ok(new
    //         {
    //             examination.Diagnostic,
    //             examination.Description,
    //             examination.State,
    //             Doctor = examination.Doctor,
    //             Appointment = examination.Appointment,
    //             Prescription = examination.Prescription,
    //             DiagnosticServices = examination.DiagnosticServices
    //         });
    //     }
    //     return NotFound("Examination not found.");
    // }

    // [HttpPost]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> CreateExamination([FromBody] CreateExaminationRequest request)
    // {
    //     if (request.AppointmentId == null)
    //         return BadRequest("Appointment ID cannot be null.");

    //     var appointment = await _repository.GetEntityBy<uint, IAppointment>(request.AppointmentId);
    //     if (appointment == null)
    //     {
    //         return NotFound("Appointment not found.");
    //     }

    //     var doctor = await _repository.GetEntityBy<uint, IDoctor>(request.DoctorId);
    //     if (doctor == null)
    //     {
    //         return NotFound("Doctor not found.");
    //     }

    //     // Tạo Examination mới
    //     var newExamination = await _repository.ObtainEntity<IExamination>();

    //     newExamination.Diagnostic = request.Diagnostic;
    //     newExamination.Description = request.Description;
    //     newExamination.State = request.State;

    //     if (!await newExamination.Create())
    //     {
    //         return BadRequest("Cannot create examination.");
    //     }

    //     return Ok(new
    //     {
    //         newExamination.Diagnostic,
    //         newExamination.Description,
    //         newExamination.State,
    //         Doctor = newExamination.Doctor,
    //         Appointment = newExamination.Appointment
    //     });
    // }

    // [HttpPut("{id}")]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> UpdateExamination(uint id, [FromBody] UpdateExaminationRequest request)
    // {
    //     var examination = await _repository.GetEntityBy<uint, IExamination>(id);

    //     if (examination == null)
    //     {
    //         return NotFound("Examination not found.");
    //     }

    //     examination.Diagnostic = request.Diagnostic ?? examination.Diagnostic;
    //     examination.Description = request.Description ?? examination.Description;
    //     examination.State = request.State ?? examination.State;

    //     if (!await examination.Update())
    //     {
    //         return BadRequest("Cannot update examination.");
    //     }
    //     return Ok("Examination updated successfully.");
    // }

    // [HttpDelete("{id}")]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> DeleteExamination(uint id)
    // {
    //     var examination = await _repository.GetEntityBy<uint, IExamination>(id);

    //     if (examination == null)
    //     {
    //         return NotFound("Examination not found.");
    //     }

    //     if (!await examination.Delete())
    //     {
    //         return BadRequest("Cannot delete examination.");
    //     }

    //     return Ok("Examination deleted successfully.");
    // }

    // [HttpPost("{id}/prescription")]
    // public async Task<ActionResult> ObtainPrescription(uint id)
    // {
    //     var examination = await _repository.GetEntityBy<uint, IExamination>(id);

    //     if (examination == null)
    //     {
    //         return NotFound("Examination not found.");
    //     }

    //     var prescription = await examination.ObtainPrescription();

    //     if (prescription == null)
    //     {
    //         return BadRequest("Cannot obtain prescription for this examination.");
    //     }

    //     return Ok(prescription);
    // }

    // [HttpPost("{id}/diagnostic")]
    // public async Task<ActionResult> AddDiagnosticService(uint id, [FromBody] AddDiagnosticServiceRequest request)
    // {
    //     var examination = await _repository.GetEntityBy<uint, IExamination>(id);

    //     if (examination == null)
    //     {
    //         return NotFound("Examination not found.");
    //     }

    //     var doctor = await _repository.GetEntityBy<uint, IDoctor>(request.DoctorId);
    //     if (doctor == null)
    //     {
    //         return NotFound("Doctor not found.");
    //     }

    //     var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(request.DiagnosticServiceId);
    //     if (diagnosticService == null)
    //     {
    //         return NotFound("Diagnostic service not found.");
    //     }

    //     var result = await examination.ObtainDiagnostic(doctor, diagnosticService);

    //     if (result == null)
    //     {
    //         return BadRequest("Cannot add diagnostic service to examination.");
    //     }

    //     return Ok(result);
    // }
}