using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IRepository repository, ILogger<ProfileController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProfiles()
    {
        try
        {
            var dbContext = await _repository.GetService<DbContext>();
            var profiles = await dbContext.Set<Profile>().ToListAsync();

            if (profiles == null || profiles.Count == 0)
            {
                return NotFound("No profiles found.");
            }

            return Ok(profiles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all profiles.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }


    [HttpGet("{id}")]
    public async Task<ActionResult> GetProfileById(uint id)
    {
        try
        {
            var dbContext = await _repository.GetService<DbContext>();
            var profile = await dbContext.FindAsync<Profile>(id);

            if (profile != null)
            {
                return Ok(profile);
            }
            return NotFound($"Profile with ID {id} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting profile with ID {id}.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    [HttpPost]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> CreateProfile([FromBody] CreateProfileRequest profile)
    {

        if (profile.PatientId == null)
            return BadRequest("Patient ID cannot be null.");
        var patient = await _repository.GetEntityBy<uint, IPatient>((uint)profile.PatientId.Value);
        if (patient == null)
        {
            return NotFound("Can not find this patient");
        }

        // Kiểm tra FullName
        if (string.IsNullOrWhiteSpace(profile.Fullname))
            return BadRequest("FullName cannot be null or empty.");
        if (profile.Fullname.Length < 5 || profile.Fullname.Length > 36)
            return BadRequest("FullName must be between 10 and 36 characters.");

        // Kiểm tra ngày sinh (BirthDate)
        if (profile.BirthDate == null)
            return BadRequest("BirthDate cannot be null.");

        // Kiểm tra xem ngày sinh có trong tương lai không
        if (profile.BirthDate > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest("BirthDate cannot be in the future.");

        // Tạo hồ sơ mới nếu các kiểm tra đã hợp lệ
        var newProfile = await patient.ObtainProfile();
        newProfile.FullName = profile.Fullname;
        newProfile.DateOfBirth = profile.BirthDate.Value;
        newProfile.Gender = profile.Gender;

        if (!await newProfile.Create())
            return BadRequest("Cannot create profile.");

        return Ok(newProfile);

    }

    [HttpPut("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> UpdateAppointment([FromBody] UpdateProfileRequest profile, uint id)
    {
        var profileExist = await _repository.GetEntityBy<uint, IProfile>(id);

        if (profileExist == null)
        {
            return NotFound("Can not find this Profile");
        }

        // Kiểm tra FullName
        if (string.IsNullOrWhiteSpace(profile.Fullname))
            return BadRequest("FullName cannot be null or empty.");
        if (profile.Fullname.Length < 5 || profile.Fullname.Length > 36)
            return BadRequest("FullName must be between 10 and 36 characters.");

        // Kiểm tra ngày sinh (BirthDate)
        if (profile.BirthDate == null)
            return BadRequest("BirthDate cannot be null.");

        // Kiểm tra xem ngày sinh có trong tương lai không
        if (profile.BirthDate > DateOnly.FromDateTime(DateTime.Today))
            return BadRequest("BirthDate cannot be in the future.");

        profileExist.FullName = profile.Fullname;
        profileExist.DateOfBirth = profile.BirthDate.Value;
        profileExist.Gender = profile.Gender;

        if (!await profileExist.Update())
        {
            return BadRequest("Can not update Profile");
        }
        return Ok("Changed");
    }


    [HttpDelete("{id}")]
    [JSONWebToken(AuthenticationRequired = false)]
    public async Task<ActionResult> UpdateProfile(uint id)
    {
        var profileExit = await _repository.GetEntityBy<uint, IProfile>(id);

        if (profileExit == null)
        {
            return NotFound("Can not to find this profile");
        }

        if (!await profileExit.Delete())
        {
            return BadRequest("Can not delete this profile");
        }

        return Ok("This profile has been delete");
    }


}
