using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Authorization;
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

		[HttpGet("part"), JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
		public async Task<ActionResult<SchedulerPart>> GetParts()
			=> Ok((await _repository.GetService<ISchedulerService>()).Parts);
	}
}
