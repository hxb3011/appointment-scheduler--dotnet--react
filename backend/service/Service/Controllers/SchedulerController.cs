using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers
{

    [ApiController, Route("api/[controller]")]
    public class SchedulerController : ControllerBase
    {
		protected readonly IRepository _repository;
        protected readonly ILogger<SchedulerController> _logger;

        public SchedulerController(IRepository repository, ILogger<SchedulerController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [JSONWebToken(AuthenticationRequired = true)]
        public async Task<ActionResult<IEnumerable<SchedulerPart>>> GetParts()
        => Ok((await _repository.GetService<ISchedulerService>()).Parts);

        [HttpGet("{id}")]
        [JSONWebToken(AuthenticationRequired = true)]
        public async Task<ActionResult<SchedulerPart>> GetScheduleById(uint id)
        {
            try
            {
                var schedulerService = await _repository.GetService<ISchedulerService>();

                var schedulerParts = schedulerService.Parts
                    .Where(sp => sp.Id == id)
                    .FirstOrDefault();

                if (schedulerParts == null)
                    return NotFound();

                return Ok(schedulerParts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching schedule by id {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
