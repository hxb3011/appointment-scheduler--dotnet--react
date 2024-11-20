using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests.Create;
using AppointmentScheduler.Domain.Requests.Update;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetExaminationById(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);

        if (examination != null)
        {
            return Ok(new
            {
                examination.Diagnostic,
                examination.Description,
                examination.State,
                Doctor = examination.Doctor,
                Appointment = examination.Appointment,
                Prescription = examination.Prescription,
                DiagnosticServices = examination.DiagnosticServices
            });
        }
        return NotFound("Examination not found.");
    }

    [HttpPost]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> CreateExamination([FromBody] CreateExaminationRequest request)
    {
        if (request.AppointmentId == null)
            return BadRequest("Appointment ID cannot be null.");

        var appointment = await _repository.GetEntityBy<uint, IAppointment>(request.AppointmentId);
        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }

        var doctor = await _repository.GetEntityBy<uint, IDoctor>(request.DoctorId);
        if (doctor == null)
        {
            return NotFound("Doctor not found.");
        }

        // Tạo Examination mới
        var newExamination = await _repository.ObtainEntity<IExamination>();

        newExamination.Diagnostic = request.Diagnostic;
        newExamination.Description = request.Description;
        newExamination.State = request.State;

        if (!await newExamination.Create())
        {
            return BadRequest("Cannot create examination.");
        }

        return Ok(new
        {
            newExamination.Diagnostic,
            newExamination.Description,
            newExamination.State,
            Doctor = newExamination.Doctor,
            Appointment = newExamination.Appointment
        });
    }

    [HttpPut("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> UpdateExamination(uint id, [FromBody] UpdateExaminationRequest request)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);

        if (examination == null)
        {
            return NotFound("Examination not found.");
        }

        examination.Diagnostic = request.Diagnostic ?? examination.Diagnostic;
        examination.Description = request.Description ?? examination.Description;
        examination.State = request.State ?? examination.State;

        if (!await examination.Update())
        {
            return BadRequest("Cannot update examination.");
        }
        return Ok("Examination updated successfully.");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> DeleteExamination(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);

        if (examination == null)
        {
            return NotFound("Examination not found.");
        }

        if (!await examination.Delete())
        {
            return BadRequest("Cannot delete examination.");
        }

        return Ok("Examination deleted successfully.");
    }

    [HttpPost("{id}/prescription")]
    public async Task<ActionResult> ObtainPrescription(uint id)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);

        if (examination == null)
        {
            return NotFound("Examination not found.");
        }

        var prescription = await examination.ObtainPrescription();

        if (prescription == null)
        {
            return BadRequest("Cannot obtain prescription for this examination.");
        }

        return Ok(prescription);
    }

    [HttpPost("{id}/diagnostic")]
    public async Task<ActionResult> AddDiagnosticService(uint id, [FromBody] AddDiagnosticServiceRequest request)
    {
        var examination = await _repository.GetEntityBy<uint, IExamination>(id);

        if (examination == null)
        {
            return NotFound("Examination not found.");
        }

        var doctor = await _repository.GetEntityBy<uint, IDoctor>(request.DoctorId);
        if (doctor == null)
        {
            return NotFound("Doctor not found.");
        }

        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(request.DiagnosticServiceId);
        if (diagnosticService == null)
        {
            return NotFound("Diagnostic service not found.");
        }

        var result = await examination.ObtainDiagnostic(doctor, diagnosticService);

        if (result == null)
        {
            return BadRequest("Cannot add diagnostic service to examination.");
        }

        return Ok(result);
    }
}