using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AppointmentScheduler.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IRepository repository, JSONWebTokenOptions jwt, IPasswordHasher<IUser> hasher, ILogger<RoleController> logger) : ControllerBase
{
    private readonly IRepository _repository = repository;
    private readonly JSONWebTokenOptions _jwt = jwt;
    private readonly IPasswordHasher<IUser> _hasher = hasher;
    private readonly ILogger<RoleController> _logger = logger;


    [HttpPost("token")]
    public async Task<ActionResult<AuthTokenResponse>> SignIn([FromForm] AuthRequest request)
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
    public async Task<ActionResult<IEnumerable<RoleResponse>>> Register([FromForm] RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}