using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IRepository _repository;
		private readonly ILogger<PatientController> _logger;

		public PatientController(IRepository repository, ILogger<PatientController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult> GetAllPatients()
		{
			var dbContext = await _repository.GetService<DbContext>();
			var patients = await dbContext.Set<Patient>().ToListAsync();

			return Ok(patients);
		}
	}
}
