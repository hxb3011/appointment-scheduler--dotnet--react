using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IRepository repository, JSONWebTokenOptions jwt, IPasswordHasher<IUser> hasher, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IRepository _repository = repository;
    private readonly JSONWebTokenOptions _jwt = jwt;
    private readonly IPasswordHasher<IUser> _hasher = hasher;
    private readonly ILogger<AuthController> _logger = logger;


    [HttpPost("token")]
    public ActionResult<AuthTokenResponse> SignIn([FromForm] AuthRequest request)
    {
        var user = _repository.GetEntities<IUser>(
            whereProperty: nameof(Domain.Entities.User.UserName),
            andValue: request.Username, areEqual: true
        ).FirstOrDefault();

        if (user == null)
            return NotFound("user not found.");

        var passwordVerificationResult = _hasher.VerifyHashedPassword(user, user.Password, request.Password);
        if (passwordVerificationResult != PasswordVerificationResult.Success
            && passwordVerificationResult != PasswordVerificationResult.SuccessRehashNeeded)
            return BadRequest("invalid pass not found.");

        return Ok(new AuthTokenResponse()
        {
            AccessToken = _jwt.GetJSONWebToken(user, _repository, _logger)
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] PatientRequest request)
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
        patient.Password = _hasher.HashPassword(patient, patient.Password);

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
}