using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests.ExaminationDetail;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationDetailController : ControllerBase
    {
        private readonly IRepository _repository;
        private ILogger<ExaminationDetailController> _logger;

        public ExaminationDetailController(IRepository repository, ILogger<ExaminationDetailController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllExaminationDetails()
        {
            var dbContext = await _repository.GetService<DbContext>();
            var examinations = await dbContext.Set<Examination>().ToListAsync();

            if (examinations.Any())
            {
                return Ok(examinations);
            }
            return NotFound("No Appointments found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetExaminationDetailById(uint id)
        {
            var dbContext = await _repository.GetService<DbContext>();
            var examinationDetail = await dbContext.FindAsync<Examination>(id);

            if (examinationDetail != null)
            {
                return Ok(examinationDetail);
            }
            return BadRequest("Error occur");
        }

        [HttpPost]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult<Examination>> CreateExaminationDetail([FromBody]CreateExamination examination)
        {
            if (examination.AppointmentId == null)
                return BadRequest("Appointment id can not be null");
            var appointment = await _repository.GetEntityBy<uint, IAppointment>((uint)examination.AppointmentId);
            if (appointment == null)
                return NotFound("Can not find this appointment");

            var newExamination = await appointment.ObtainExamination();

            newExamination.Diagnostic = examination.Diagnostic;
            newExamination.Description = examination.Description;

            if (!await newExamination.Create())
                return BadRequest("Can not create appointment");

            return Ok(newExamination);
        }

        [HttpPut("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> UpdateExaminationDetail([FromBody]Examination updateExamination, uint id)
        {
            var change = false;

            var examinationExist = await _repository.GetEntityBy<uint, IExamination>(id);

            if (examinationExist == null)
                return NotFound("Can not find this examination detail");

            var appointment = await _repository.GetEntityBy<uint, IAppointment>(updateExamination.AppointmentId);
            
            if (updateExamination.AppointmentId != 0 && examinationExist.Appointment.Equals(appointment))
            {
                change = true;
                if (appointment == null)
                    return NotFound("Can not find this appointment");
            }

            if (updateExamination.Diagnostic != null && updateExamination.Diagnostic != examinationExist.Diagnostic)
            {
                examinationExist.Diagnostic = updateExamination.Diagnostic;
                change = true;
            }

            if (updateExamination.Description != null && updateExamination.Description != examinationExist.Description)
            {
                examinationExist.Description = updateExamination.Description;
                change = true;
            }

            if (updateExamination.State != examinationExist.State)
            {
                examinationExist.State = (uint)updateExamination.State;
                change = true;
            }

            if (change)
            {
                if(!await examinationExist.Update())
                {
                    return BadRequest("Can not update examination");
                }
                return Ok("Changed successfull");
            }
            return Ok("No change");
        }


        [HttpDelete("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> DeleteExamination(uint id)
        {
            var examinationExist = await _repository.GetEntityBy<uint, IExamination>(id);

            if (examinationExist == null)
            {
                return NotFound("Can not find this examination");
            }

            if (!await examinationExist.Delete())
            {
                return BadRequest("Can not delete this examination");
            }

            return Ok("This examination has been delete");
        }

    }
}
