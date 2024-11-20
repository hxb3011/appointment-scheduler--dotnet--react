using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicineController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<MedicineController> _logger;

    public MedicineController(IRepository repository, ILogger<MedicineController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetMedicineById(uint id)
    {
        var medicine = await _repository.GetEntityBy<uint, IMedicine>(id);
        if (medicine != null)
        {
            return Ok(medicine);
        }
        return BadRequest("Error occur");
    }

}