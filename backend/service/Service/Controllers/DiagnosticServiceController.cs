using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests.Create;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosticServiceController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<DiagnosticServiceController> _logger;

    public DiagnosticServiceController(IRepository repository, ILogger<DiagnosticServiceController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllDiagnosticServices()
    {
        var dbContext = await _repository.GetService<DbContext>();
        var diagnosticServices = await dbContext.Set<DiagnosticService>().ToListAsync();

        if (diagnosticServices.Any())
        {
            return Ok(diagnosticServices);
        }
        return NotFound("Can not find any diagnostic service");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetDiagnosticServiceById(uint id)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);

        if (diagnosticService != null)
        {
            return Ok(diagnosticService);
        }
        return NotFound("Diagnostic service not found.");
    }

    [HttpPost]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> CreateDiagnosticService([FromBody] CreateDiagnosticRequest request)
    {
        if (request.AppointmentId == null)
            return BadRequest("Appointment ID cannot be null.");

        var appointment = await _repository.GetEntityBy<uint, IAppointment>(request.AppointmentId);
        if (appointment == null)
        {
            return NotFound("Appointment not found.");
        }

        var newDiagnosticService = await _repository.ObtainEntity<IDiagnosticService>();
        newDiagnosticService.Name = request.Name;
        newDiagnosticService.Price = request.Price;

        if (!await newDiagnosticService.Create())
        {
            return BadRequest("Cannot create diagnostic service.");
        }
        return Ok(newDiagnosticService);
    }

    [HttpPut("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> UpdateDiagnosticService(uint id, [FromBody] CreateDiagnosticRequest request)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);

        if (diagnosticService == null)
        {
            return NotFound("Diagnostic service not found.");
        }

        diagnosticService.Name = request.Name ?? diagnosticService.Name;
        diagnosticService.Price = request.Price;

        if (!await diagnosticService.Update())
        {
            return BadRequest("Cannot update diagnostic service.");
        }
        return Ok("Diagnostic service updated successfully.");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> DeleteDiagnosticService(uint id)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);

        if (diagnosticService == null)
        {
            return NotFound("Diagnostic service not found.");
        }

        if (!await diagnosticService.Delete())
        {
            return BadRequest("Cannot delete diagnostic service.");
        }

        return Ok("Diagnostic service deleted successfully.");
    }
}
