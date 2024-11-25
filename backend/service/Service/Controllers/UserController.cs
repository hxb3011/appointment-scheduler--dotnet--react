using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		protected readonly IRepository _repository;
		protected readonly IPasswordHasher<IUser> _passwordHasher;
		protected readonly ILogger<PatientController> _logger;

		public UserController(IRepository repository,
			IPasswordHasher<IUser> passwordHasher, ILogger<PatientController> logger)
		{
			_repository = repository;
			_passwordHasher = passwordHasher;
			_logger = logger;
		}

		[HttpPut("changePasword")]
		[JSONWebToken(RequiredPermissions = [Permission.UpdateUser])]
		public async Task<ActionResult> ChangePasword(ChangePasswordRequest request)
		{
			var user = HttpContext.GetAuthUser();
			var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.OldPassword);
			if (result == PasswordVerificationResult.Success
				|| result == PasswordVerificationResult.SuccessRehashNeeded)
				return BadRequest("old password not match");

			user.Password = request.NewPassword;
			if (!user.IsPasswordValid)
				return BadRequest("new password not valid");
			user.Password = _passwordHasher.HashPassword(user, user.Password);

			if (!await user.Update())
				return BadRequest("can not update");

			return Ok("success");
		}
	}
}
