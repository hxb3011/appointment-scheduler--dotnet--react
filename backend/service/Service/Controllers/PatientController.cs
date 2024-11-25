using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
	public class PatientController : UserController
	{
		public PatientController(IRepository repository,
			IPasswordHasher<IUser> passwordHasher, ILogger<PatientController> logger)
			: base(repository, passwordHasher, logger) { }

		private PatientResponse MakeResponse(IPatient patient)
			=> !_repository.TryGetKeyOf(patient, out uint id) ? null
			: new() { Id = id, UserName = patient.UserName, FullName = patient.FullName, Email = patient.Email, Phone = patient.Phone };

		[HttpGet]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadUser])]
		public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPagedPatients([FromBody] PagedGetAllRequest request)
			=> Ok(_repository.GetEntities<IPatient>(request.Offset, request.Count, request.By).Select(MakeResponse));

		[HttpGet("{id}")]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadUser])]
		public async Task<ActionResult<PatientResponse>> GetPatient(uint id)
		{
			var patient = await _repository.GetEntityBy<uint, IPatient>(id);
			if (patient == null) return NotFound();
			return Ok(MakeResponse(patient));
		}

		[HttpPost]
		[JSONWebToken(RequiredPermissions = [Permission.CreateUser])]
		public async Task<ActionResult> CreatePatient([FromBody] PatientRequest request)
		{
			var patient = await _repository.ObtainEntity<IPatient>();
			if (patient == null)
				return BadRequest("can not create");

			patient.UserName = request.Username;
			if (!patient.IsUserNameValid)
				return BadRequest("username not valid");
			if (await patient.IsUserNameExisted())
				return BadRequest("username existed");

			patient.FullName = request.FullName;
			if (!patient.IsFullNameValid)
				return BadRequest("full_name not valid");

			patient.Password = request.Password;
			if (!patient.IsPasswordValid)
				return BadRequest("password not valid");
			patient.Password = _passwordHasher.HashPassword(patient, patient.Password);

			patient.Email = request.Email;
			if (!patient.IsEmailValid)
				return BadRequest("email not valid");

			patient.Phone = request.Phone;
			if (!patient.IsPhoneValid)
				return BadRequest("phone not valid");

			if (!await patient.Create())
				return BadRequest("can not create");

			return Ok("success");
		}

		[HttpPut("{id}")]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
		public async Task<ActionResult> UpdatePatient([FromBody] PatientRequest request, uint id)
		{
			var patient = await _repository.GetEntityBy<uint, IPatient>(id);
			if (patient == null) return NotFound();
			string v;
			if ((v = request.Username) != null)
			{
				patient.UserName = v;
				if (!patient.IsUserNameValid)
					return BadRequest("username not valid");
				if (await patient.IsUserNameExisted())
					return BadRequest("username existed");
			}
			if ((v = request.FullName) != null)
			{
				patient.FullName = v;
				if (!patient.IsFullNameValid)
					return BadRequest("full_name not valid");
			}
			if ((v = request.Password) != null)
			{
				patient.Password = v;
				if (!patient.IsPasswordValid)
					return BadRequest("password not valid");
				patient.Password = _passwordHasher.HashPassword(patient, patient.Password);
			}
			if ((v = request.Email) != null)
			{
				patient.Email = v;
				if (!patient.IsEmailValid)
					return BadRequest("email not valid");
			}
			if ((v = request.Phone) != null)
			{
				patient.Phone = v;
				if (!patient.IsPhoneValid)
					return BadRequest("phone not valid");
			}
			if (!await patient.Create())
				return BadRequest("can not create");
			return Ok("success");
		}

		[HttpDelete("{id}")]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.DeleteUser])]
		public async Task<ActionResult> DeletePatient(uint id)
		{
			var patient = await _repository.GetEntityBy<uint, IPatient>(id);
			if (patient == null) return NotFound();
			if (!await patient.Delete())
				return BadRequest("can not delete");
			return Ok("success");
		}

		[HttpGet("{id}/image")]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
		public async Task<ActionResult> GetImage(uint id)
		{
			var patient = await _repository.GetEntityBy<uint, IPatient>(id);
			if (patient == null) return NotFound();
			return File(patient.Image(readOnly: true), "application/octet-stream");
		}
		
		[HttpPost("{id}/image")]
		[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
		public async Task<ActionResult> SetImage(uint id, IFormFile file)
		{
			var patient = await _repository.GetEntityBy<uint, IPatient>(id);
			if (patient == null) return NotFound();
			if (file == null || file.Length == 0)
				return BadRequest("file invalid");
			await file.CopyToAsync(patient.Image(readOnly: false));
			return Ok("success");
		}

		[HttpGet]
		[JSONWebToken(RequiredPermissions = [Permission.ReadUser])]
		public async Task<ActionResult<PatientResponse>> GetCurrentUser()
		{
			if (HttpContext.GetAuthUser() is not IPatient patient) return NotFound();
			return Ok(MakeResponse(patient));
		}

		[HttpPut]
		[JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
		public async Task<ActionResult> UpdateCurrentUser([FromBody] PatientRequest request)
		{
			if (HttpContext.GetAuthUser() is not IPatient patient) return NotFound();
			string v;
			if ((v = request.Username) != null)
			{
				patient.UserName = v;
				if (!patient.IsUserNameValid)
					return BadRequest("username not valid");
				if (await patient.IsUserNameExisted())
					return BadRequest("username existed");
			}
			if ((v = request.FullName) != null)
			{
				patient.FullName = v;
				if (!patient.IsFullNameValid)
					return BadRequest("full_name not valid");
			}
			if ((v = request.Password) != null)
			{
				patient.Password = v;
				if (!patient.IsPasswordValid)
					return BadRequest("password not valid");
				patient.Password = _passwordHasher.HashPassword(patient, patient.Password);
			}
			if ((v = request.Email) != null)
			{
				patient.Email = v;
				if (!patient.IsEmailValid)
					return BadRequest("email not valid");
			}
			if ((v = request.Phone) != null)
			{
				patient.Phone = v;
				if (!patient.IsPhoneValid)
					return BadRequest("phone not valid");
			}
			if (!await patient.Create())
				return BadRequest("can not create");
			return Ok("success");
		}

        [HttpGet("image")]
        [JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
        public ActionResult GetImage()
        {
            if (HttpContext.GetAuthUser() is not IPatient patient) return NotFound();
            return File(patient.Image(readOnly: true), "application/octet-stream");
        }

        [HttpPost("image")]
		[JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
		public async Task<ActionResult> SetImage(IFormFile file)
		{
			if (HttpContext.GetAuthUser() is not IPatient patient) return NotFound();
			if (file == null || file.Length == 0)
				return BadRequest("file invalid");
			await file.CopyToAsync(patient.Image(readOnly: false));
			return Ok("success");
		}
	}
}
