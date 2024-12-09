using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorController : UserController
{
	public DoctorController(IRepository repository,
		IPasswordHasher<IUser> passwordHasher, ILogger<DoctorController> logger)
		: base(repository, passwordHasher, logger) { }

	private DoctorResponse MakeResponse(IDoctor doctor)
		=> !_repository.TryGetKeyOf(doctor, out Doctor d) || !_repository.TryGetKeyOf(doctor, out User u) ? null
		: new() { Id = u.Id, UserName = u.UserName, FullName = u.FullName, Email = d.Email, Phone = d.Phone, Certificate = d.Certificate, Position = d.Position, Role = u.RoleId };

	[HttpGet]
	[JSONWebToken(RequiredPermissions = [Permission.ReadUser])]
	public ActionResult<IEnumerable<DoctorResponse>> GetPagedDoctors([FromQuery] PagedGetAllRequest request)
		=> Ok(_repository.GetEntities<IDoctor>(request.Offset, request.Count, request.By).Select(MakeResponse));

	[HttpGet("{id}")]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadUser])]
	public async Task<ActionResult<DoctorResponse>> GetDoctor(uint id)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		return Ok(MakeResponse(doctor));
	}

	[HttpPost]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.CreateUser])]
	public async Task<ActionResult> CreateDoctor([FromBody] DoctorRequest request)
	{
		var doctor = await _repository.ObtainEntity<IDoctor>();
		if (doctor == null)
			return BadRequest("can not create");

		doctor.UserName = request.Username;
		if (!doctor.IsUserNameValid)
			return BadRequest("username not valid");
		if (await doctor.IsUserNameExisted())
			return BadRequest("username existed");

		doctor.FullName = request.FullName;
		if (!doctor.IsFullNameValid)
			return BadRequest("full_name not valid");

		doctor.Password = request.Password;
		if (!doctor.IsPasswordValid)
			return BadRequest("password not valid");
		doctor.Password = _passwordHasher.HashPassword(doctor, doctor.Password);

		doctor.Email = request.Email;
		if (!doctor.IsEmailValid)
			return BadRequest("email not valid");

		doctor.Phone = request.Phone;
		if (!doctor.IsPhoneValid)
			return BadRequest("phone not valid");

		doctor.Certificate = request.Certificate;
		doctor.Position = request.Position;

		if (!await doctor.Create())
			return BadRequest("can not create");

		return Ok("success");
	}

	[HttpPut("{id}")]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
	public async Task<ActionResult> UpdateDoctor([FromBody] DoctorRequest request, uint id)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		string v;
		uint r;
		if ((v = request.Username) != null)
		{
			doctor.UserName = v;
			if (!doctor.IsUserNameValid)
				return BadRequest("username not valid");
			if (await doctor.IsUserNameExisted())
				return BadRequest("username existed");
		}
		if ((v = request.FullName) != null)
		{
			doctor.FullName = v;
			if (!doctor.IsFullNameValid)
				return BadRequest("full_name not valid");
		}
		if ((v = request.Password) != null)
		{
			doctor.Password = v;
			if (!doctor.IsPasswordValid)
				return BadRequest("password not valid");
			doctor.Password = _passwordHasher.HashPassword(doctor, doctor.Password);
		}
        if ((v = request.Email) != null)
		{
			doctor.Email = v;
			if (!doctor.IsEmailValid)
				return BadRequest("email not valid");
		}
		if ((v = request.Phone) != null)
		{
			doctor.Phone = v;
			if (!doctor.IsPhoneValid)
				return BadRequest("phone not valid");
		}

		if(request.RoleId != 0)
		{
            var role = await _repository.GetEntityBy<uint, IRole>(request.RoleId);
            await doctor.ChangeRole(role);
        }
		

		if ((v = request.Certificate) != null)
			doctor.Certificate = v;
		if ((v = request.Position) != null)
			doctor.Position = v;
		if (!await doctor.Update())
			return BadRequest("can not update");
		return Ok("success");
	}

	[HttpDelete("{id}")]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.DeleteUser])]
	public async Task<ActionResult> DeleteDoctor(uint id)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		if (!await doctor.Delete())
			return BadRequest("can not delete");
		return Ok("success");
	}

    private ExaminationDiagnosticResponse MakeExaminationDiagnosticResponse(IDiagnosticService examinationDiagnostic)
        => !_repository.TryGetKeyOf(examinationDiagnostic, out ExaminationService key) ? null
        : new() { Name = examinationDiagnostic.Name, Price = examinationDiagnostic.Price, Doctor = key.DoctorId, DiagnosticService = key.DiagnosticServiceId, Examination = key.ExaminationId };

	[HttpGet("{id}/examdiag")]
	[JSONWebToken(RequiredPermissions = [Permission.ReadDiagnosticService])]
	public async Task<ActionResult<IEnumerable<ExaminationDiagnosticResponse>>> GetPagedExaminationDiagnostics([FromQuery] PagedGetAllRequest request, uint id)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		return Ok(doctor.DiagnosticServices.AsQueryable().OrderByPropertyName(request.By)
			.Skip(request.Offset).Take(request.Count).Select(MakeExaminationDiagnosticResponse));
	}

	[HttpGet("{id}/image")]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
	public async Task<ActionResult> GetImage(uint id)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		return File(doctor.Image(readOnly: true), "application/octet-stream");
	}

	[HttpPost("{id}/image")]
	[JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateUser])]
	public async Task<ActionResult> SetImage(uint id, IFormFile file)
	{
		var doctor = await _repository.GetEntityBy<uint, IDoctor>(id);
		if (doctor == null) return NotFound();
		if (file == null || file.Length == 0)
			return BadRequest("file invalid");
		await file.CopyToAsync(doctor.Image(readOnly: false));
		return Ok("success");
	}

	[HttpGet("current")]
	[JSONWebToken(RequiredPermissions = [Permission.ReadUser])]
	public ActionResult<DoctorResponse> GetCurrentUser()
	{
		if (HttpContext.GetAuthUser() is not IDoctor doctor) return NotFound();
		return Ok(MakeResponse(doctor));
	}

	[HttpPut("current")]
	[JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
	public async Task<ActionResult> UpdateCurrentUser([FromBody] DoctorRequest request)
	{
		if (HttpContext.GetAuthUser() is not IDoctor doctor) return NotFound();
		string v;
		if ((v = request.Username) != null)
		{
			doctor.UserName = v;
			if (!doctor.IsUserNameValid)
				return BadRequest("username not valid");
			if (await doctor.IsUserNameExisted())
				return BadRequest("username existed");
		}
		if ((v = request.FullName) != null)
		{
			doctor.FullName = v;
			if (!doctor.IsFullNameValid)
				return BadRequest("full_name not valid");
		}
		if ((v = request.Password) != null)
		{
			doctor.Password = v;
			if (!doctor.IsPasswordValid)
				return BadRequest("password not valid");
			doctor.Password = _passwordHasher.HashPassword(doctor, doctor.Password);
		}
		if ((v = request.Email) != null)
		{
			doctor.Email = v;
			if (!doctor.IsEmailValid)
				return BadRequest("email not valid");
		}
		if ((v = request.Phone) != null)
		{
			doctor.Phone = v;
			if (!doctor.IsPhoneValid)
				return BadRequest("phone not valid");
		}
		if (!await doctor.Create())
			return BadRequest("can not create");
		return Ok("success");
	}

	[HttpGet("current/image")]
	[JSONWebToken(RequiredPermissions = [Permission.ReadUser])]
	public ActionResult GetImage()
	{
		if (HttpContext.GetAuthUser() is not IDoctor doctor) return NotFound();
		return File(doctor.Image(readOnly: true), "application/octet-stream");
	}

	[HttpPost("current/image")]
	[JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
	public async Task<ActionResult> SetImage(IFormFile file)
	{
		if (HttpContext.GetAuthUser() is not IDoctor doctor) return NotFound();
		if (file == null || file.Length == 0)
			return BadRequest("file invalid");
		await file.CopyToAsync(doctor.Image(readOnly: false));
		return Ok("success");
	}
}
