using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    private ProfileResponse MakeResponse(IProfile role)
        => !_repository.TryGetKeyOf(role, out Profile key) ? null
        : new() { Id = key.Id, Patient = key.PatientId, FullName = key.FullName, DateOfBirth = key.DateOfBirth, Gender = key.Gender };

    [HttpGet]
    [JSONWebToken(RequiredPermissions = [Permission.ReadProfile])]
    public ActionResult<IEnumerable<ProfileResponse>> GetPagedProfiles([FromBody] PagedGetAllRequest request)
        => Ok(_repository.GetEntities<IProfile>(request.Offset, request.Count, request.By).Select(MakeResponse));

    [HttpGet("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadProfile])]
    public async Task<ActionResult<ProfileResponse>> GetProfile(uint id)
    {
        var profile = await _repository.GetEntityBy<uint, IProfile>(id);
        if (profile == null) return NotFound();
        return Ok(MakeResponse(profile));
    }

    [HttpPost]
    [JSONWebToken(RequiredPermissions = [Permission.CreateProfile])]
    public async Task<ActionResult> CreateProfile([FromBody] ProfileRequest request)
    {
        var patient = await _repository.GetEntityBy<uint, IPatient>(request.Patient);
        if (patient == null) return NotFound("patient not found");
        var profile = await patient.ObtainProfile();
        if (profile == null) return BadRequest("can not create");

        profile.FullName = request.FullName;
        if (!profile.IsFullNameValid)
            return BadRequest("full_name not valid");

        profile.DateOfBirth = request.DateOfBirth;
        if (profile.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            return BadRequest("date_of_birth not valid");

        profile.Gender = request.Gender;

        if (!await profile.Create())
            return BadRequest("can not create");

        return Ok("success");
    }

    [HttpPut("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.UpdateProfile])]
    public async Task<ActionResult> UpdateProfile([FromBody] ProfileRequest request, uint id)
    {
        var profile = await _repository.GetEntityBy<uint, IProfile>(id);
        if (profile == null) return NotFound("profile not found");

        string v;
        if ((v = request.FullName) != null)
        {
            profile.FullName = v;
            if (!profile.IsFullNameValid)
                return BadRequest("full_name not valid");
        }

        profile.DateOfBirth = request.DateOfBirth;
        if (profile.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            return BadRequest("date_of_birth not valid");

        profile.Gender = request.Gender;

        if (!await profile.Update())
            return BadRequest("can not update");

        return Ok("success");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.DeleteProfile])]
    public async Task<ActionResult> DeleteProfile(uint id)
    {
        var profile = await _repository.GetEntityBy<uint, IProfile>(id);
        if (profile == null) return NotFound();

        if (!await profile.Delete())
            return BadRequest("can not delete");

        return Ok("success");
    }

    // [HttpPost]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> CreateProfile([FromBody] CreateProfileRequest profile)
    // {

    //     if (profile.PatientId == null)
    //         return BadRequest("Patient ID cannot be null.");
    //     var patient = await _repository.GetEntityBy<uint, IPatient>((uint)profile.PatientId.Value);
    //     if (patient == null)
    //     {
    //         return NotFound("Can not find this patient");
    //     }

    //     // Kiểm tra FullName
    //     if (string.IsNullOrWhiteSpace(profile.Fullname))
    //         return BadRequest("FullName cannot be null or empty.");
    //     if (profile.Fullname.Length < 5 || profile.Fullname.Length > 36)
    //         return BadRequest("FullName must be between 10 and 36 characters.");

    //     // Kiểm tra ngày sinh (BirthDate)
    //     if (profile.BirthDate == null)
    //         return BadRequest("BirthDate cannot be null.");

    //     // Kiểm tra xem ngày sinh có trong tương lai không
    //     if (profile.BirthDate > DateOnly.FromDateTime(DateTime.Today))
    //         return BadRequest("BirthDate cannot be in the future.");

    //     // Tạo hồ sơ mới nếu các kiểm tra đã hợp lệ
    //     var newProfile = await patient.ObtainProfile();
    //     newProfile.FullName = profile.Fullname;
    //     newProfile.DateOfBirth = profile.BirthDate.Value;
    //     newProfile.Gender = profile.Gender;

    //     if (!await newProfile.Create())
    //         return BadRequest("Cannot create profile.");

    //     return Ok(newProfile);

    // }

    // [HttpPut("{id}")]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> UpdateAppointment([FromBody] UpdateProfileRequest profile, uint id)
    // {
    //     var profileExist = await _repository.GetEntityBy<uint, IProfile>(id);

    //     if (profileExist == null)
    //     {
    //         return NotFound("Can not find this Profile");
    //     }

    //     // Kiểm tra FullName
    //     if (string.IsNullOrWhiteSpace(profile.Fullname))
    //         return BadRequest("FullName cannot be null or empty.");
    //     if (profile.Fullname.Length < 5 || profile.Fullname.Length > 36)
    //         return BadRequest("FullName must be between 10 and 36 characters.");

    //     // Kiểm tra ngày sinh (BirthDate)
    //     if (profile.BirthDate == null)
    //         return BadRequest("BirthDate cannot be null.");

    //     // Kiểm tra xem ngày sinh có trong tương lai không
    //     if (profile.BirthDate > DateOnly.FromDateTime(DateTime.Today))
    //         return BadRequest("BirthDate cannot be in the future.");

    //     profileExist.FullName = profile.Fullname;
    //     profileExist.DateOfBirth = profile.BirthDate.Value;
    //     profileExist.Gender = profile.Gender;

    //     if (!await profileExist.Update())
    //     {
    //         return BadRequest("Can not update Profile");
    //     }
    //     return Ok("Changed");
    // }
}
