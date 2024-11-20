using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IRepository _repository;
		private readonly ILogger<DoctorController> _logger;

		public DoctorController(IRepository repository, ILogger<DoctorController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult> GetAllDoctors()
		{
			var dbContext = await _repository.GetService<DbContext>();
			var doctors = await dbContext.Set<Doctor>().ToListAsync();

			return Ok(doctors);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> GetDoctorById(uint id)
		{
			var dbContext = await _repository.GetService<DbContext>();
			var doctor = await dbContext.FindAsync<Doctor>(id);

			if(doctor != null)
			{
				return Ok(doctor);
			}
			return BadRequest("Error occur");
		}

		// TODO: Linq bug

		[HttpPost]
		[JSONWebToken(AuthenticationRequired = false)]
		public async Task<ActionResult> CreateDoctor([FromBody] Doctor doctor)
		{
			var newDoctor = await _repository.ObtainEntity<IDoctor>();

			newDoctor.Email = doctor.Email;
			newDoctor.Phone = doctor.Phone;
			newDoctor.Position = doctor.Position;
			newDoctor.Certificate = doctor.Certificate;
			newDoctor.Image = doctor.Image;

			if(!await newDoctor.Create())
			{
				return BadRequest("Cannot create doctor");
			}

			return Ok("Create new doctor successful");
		}
	}
}
