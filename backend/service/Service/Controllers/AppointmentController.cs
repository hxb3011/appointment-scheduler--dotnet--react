using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(IRepository repository, ILogger<AppointmentController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppointmentById(uint id)
        {
            var dbContext = await _repository.GetService<DbContext>();
            var appointment = await dbContext.FindAsync<Appointment>(id);

            if (appointment != null)
            {
                return Ok(appointment);
            }
            return BadRequest("Error occur");
        }

        [HttpPost]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> CreateAppointment([FromBody]CreateAppointmentRequest appointment)
        {
            if(appointment.ProfileId == null)
                return BadRequest("Profile id can not be null");
            var profile = await _repository.GetEntityBy<uint, IProfile>(appointment.ProfileId.Value);
            //var profile = await _repository.GetEntityBy<uint, IProfile>((uint)appointment.ProfileId);
            if(profile == null)
            {
                return NotFound("Can not find this profile");
            }
            if(appointment.DoctorId == null)
                return BadRequest("Doctor id can not be null");
            var doctor = await _repository.GetEntityBy<uint, IDoctor>((uint)appointment.DoctorId);
            if (doctor == null)
            {
                return NotFound("Can not find this doctor");
            }
            
            var newAppointment = await profile.ObtainAppointment(appointment.AtTime, doctor);

            if(!await newAppointment.Create())
            {
                return BadRequest("Can not create appointment");
            }
            return Ok(newAppointment);
        }

        [HttpPut("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> UpdateAppointment([FromBody]UpdateAppointmentRequest appointment, uint id)
        {
            var appointmentExist = await _repository.GetEntityBy<uint, IAppointment>(id);

            if(appointmentExist == null)
            {
                return NotFound("Can not find this appointment");
            }

            var change = false;

            var profile = await _repository.GetEntityBy<uint, IProfile>(appointment.ProfileId.Value);
            //var profile = await _repository.GetEntityBy<uint, IProfile>((uint)appointment.ProfileId);
            if (profile != null)
            {
                change = true;
                appointmentExist.Profile = profile;
            }

            
            if(appointment.State != null)
            {
                change = true;
                appointmentExist.State = (uint)appointment.State;
            }


            if (change)
            {
                if (!await appointmentExist.Update())
                {
                    return BadRequest("Can not update appointment");
                }
                return Ok("Changed");
            }
            return Ok("No change");
        }


        [HttpDelete("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> UpdateAppointment(uint id)
        {
            var appointmentExist = await _repository.GetEntityBy<uint, IAppointment>(id);

            if (appointmentExist == null)
            {
                return NotFound("Can not to find this appointment");
            }

            if (!await appointmentExist.Delete())
            {
                return BadRequest("Can not delete this appointment");
            }

            return Ok("This appointment has been delete");
        }



    }
}
