using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[ApiController, Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IRepository repository, ILogger<AppointmentController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private AppointmentResponse MakeResponse(IAppointment appointment)
        => !_repository.TryGetKeyOf(appointment, out Appointment key) ? null
        : new() { Id = key.Id, AtTime = key.AtTime, Number = key.Number, State = key.State, Profile = key.ProfileId, Doctor = key.DoctorId };

    [HttpGet]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public ActionResult<IEnumerable<AppointmentResponse>> GetPagedAppointments([FromQuery] PagedGetAllRequest request)
    {
        var user = HttpContext.GetAuthUser();
        if (!_repository.TryGetKeyOf(user, out Doctor key))
        {
            return BadRequest("Authorized user is not a doctor");
        }

        if (user.Role.IsPermissionGranted(Permission.SystemPrivilege))
        {
            return Ok(_repository.GetEntities<IAppointment>(request.Offset, request.Count, request.By).Select(MakeResponse));
        }



        return Ok(_repository.GetEntities<IAppointment>(request.Offset, request.Count, request.By, whereProperty: nameof(Appointment.DoctorId), andValue: key.Id).Select(MakeResponse));
    }

    [HttpGet("byDoctor")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public ActionResult<IEnumerable<AppointmentResponse>> GetPagedAppointmentsByDoctor([FromQuery] PagedGetAllRequest request, uint doctor)
        => Ok(_repository.GetEntities<IAppointment>(
            request.Offset, request.Count, request.By,
            whereProperty: nameof(Appointment.DoctorId), andValue: doctor
        ).Select(MakeResponse));

    [HttpGet("byProfile")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public ActionResult<IEnumerable<AppointmentResponse>> GetPagedAppointmentsByProfile([FromQuery] PagedGetAllRequest request, uint profile)
        => Ok(_repository.GetEntities<IAppointment>(
            request.Offset, request.Count, request.By,
            whereProperty: nameof(Appointment.ProfileId), andValue: profile
        ).Select(MakeResponse));

    [HttpGet("byPatient")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetPagedAppointmentsByPatient([FromQuery] PagedGetAllRequest request, uint patient)
    {
        var p = await _repository.GetEntityBy<uint, IPatient>(patient);
        if (p == null) return NotFound();
        var now = DateTime.Now;
        return Ok(p.Appointments.AsQueryable().OrderByPropertyName(request.By)
            .Skip(request.Offset).Take(request.Count).Select(MakeResponse));
    }

    [HttpGet("patient/current")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public ActionResult<IEnumerable<AppointmentResponse>> GetPagedAppointmentsByCurrentPatient([FromQuery] PagedGetAllRequest request)
    {
        var p = HttpContext.GetAuthUser();
        if (p is not IPatient patient) return Forbid("signed in user is not a patient");
        var now = DateTime.Now;
        return Ok(patient.Appointments.AsQueryable().OrderByPropertyName(request.By)
            .Skip(request.Offset).Take(request.Count).Select(MakeResponse));
    }

    [HttpGet("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadAppointment])]
    public async Task<ActionResult<AppointmentResponse>> GetAppointment(uint id)
    {
        var appointment = await _repository.GetEntityBy<uint, IAppointment>(id);
        if (appointment == null) return NotFound();
        return Ok(MakeResponse(appointment));
    }

    [HttpPost]
    [JSONWebToken(RequiredPermissions = [Permission.CreateAppointment])]
    public async Task<ActionResult> CreateAppointment([FromBody] AppointmentRequest request)
    {
        try
        {
            var doctor = await _repository.GetEntityBy<uint, IDoctor>(request.Doctor);
            if (doctor == null) return NotFound("doctor not found");
            var scheduler = await _repository.GetService<ISchedulerService>();
            var allocation = await scheduler.Allocate(doctor, request.Date, request.BeginTime, request.EndTime);

            var appointment = await doctor.ObtainAppointment(
                new DateTime(request.Date, allocation.AtTime), allocation.Id);
            if (appointment == null) return BadRequest("can not create");
            if (request.Profile.HasValue)
            {
                var profile = await _repository.GetEntityBy<uint, IProfile>(request.Profile.Value);
                if (profile == null) return NotFound("profile not found");
                appointment.Profile = profile;
            }

            if (!await appointment.Create())
                return BadRequest("can not create");
            return Ok("success");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return BadRequest(ex.ToString());
        }
        
    }

    [HttpPut("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.CreateAppointment])]
    public async Task<ActionResult> ChangeAppointmentProfile(uint id, uint profileId)
    {
        var appointment = await _repository.GetEntityBy<uint, IAppointment>(id);
        if (appointment == null) return NotFound();
        var profile = await _repository.GetEntityBy<uint, IProfile>(profileId);
        if (profile == null) return NotFound();
        appointment.Profile = profile;
        if (!await appointment.Update())
            return BadRequest("can not update");
        return Ok("success");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.DeleteAppointment])]
    public async Task<ActionResult> DeleteAppointment(uint id)
    {
        var appointment = await _repository.GetEntityBy<uint, IAppointment>(id);
        if (appointment == null) return NotFound();
        if (!await appointment.Delete())
            return BadRequest("can not delete");
        return Ok("success");
    }

    // [HttpPost]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> CreateAppointment([FromBody] CreateAppointmentRequest appointment)
    // {
    //     if (appointment.ProfileId == null)
    //         return BadRequest("Profile id can not be null");
    //     var profile = await _repository.GetEntityBy<uint, IProfile>(appointment.ProfileId.Value);
    //     //var profile = await _repository.GetEntityBy<uint, IProfile>((uint)appointment.ProfileId);
    //     if (profile == null)
    //     {
    //         return NotFound("Can not find this profile");
    //     }
    //     var doctor = await _repository.GetEntityBy<uint, IDoctor>(appointment.DoctorId);
    //     if (doctor == null)
    //     {
    //         return NotFound("Can not find this doctor");
    //     }

    //     var newAppointment = await profile.ObtainAppointment(appointment.AtTime, 0, doctor);

    //     if (!await newAppointment.Create())
    //     {
    //         return BadRequest("Can not create appointment");
    //     }
    //     return Ok(newAppointment);
    // }

    // [HttpPut("{id}")]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> UpdateAppointment([FromBody] UpdateAppointmentRequest appointment, uint id)
    // {
    //     var appointmentExist = await _repository.GetEntityBy<uint, IAppointment>(id);

    //     if (appointmentExist == null)
    //     {
    //         return NotFound("Can not find this appointment");
    //     }

    //     var change = false;

    //     var profile = await _repository.GetEntityBy<uint, IProfile>(appointment.ProfileId.Value);
    //     //var profile = await _repository.GetEntityBy<uint, IProfile>((uint)appointment.ProfileId);
    //     if (profile != null)
    //     {
    //         change = true;
    //         appointmentExist.Profile = profile;
    //     }


    //     if (appointment.State != null)
    //     {
    //         change = true;
    //         appointmentExist.State = (uint)appointment.State;
    //     }


    //     if (change)
    //     {
    //         if (!await appointmentExist.Update())
    //         {
    //             return BadRequest("Can not update appointment");
    //         }
    //         return Ok("Changed");
    //     }
    //     return Ok("No change");
    // }

    // [HttpDelete("{id}")]
    // [JSONWebToken(AuthenticationRequired = false)]
    // public async Task<ActionResult> UpdateAppointment(uint id)
    // {
    //     var appointmentExist = await _repository.GetEntityBy<uint, IAppointment>(id);

    //     if (appointmentExist == null)
    //     {
    //         return NotFound("Can not to find this appointment");
    //     }

    //     if (!await appointmentExist.Delete())
    //     {
    //         return BadRequest("Can not delete this appointment");
    //     }

    //     return Ok("This appointment has been delete");
    // }
}
