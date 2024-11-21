using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreloadController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IPasswordHasher<IUser> _passwordHasher;
    private readonly ILogger<PreloadController> _logger;

    public PreloadController(IRepository repository, IPasswordHasher<IUser> passwordHasher, ILogger<PreloadController> logger)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            await _repository.Preload(_passwordHasher, _logger);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return NotFound();
    }
}
