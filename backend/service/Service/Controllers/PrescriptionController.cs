using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<MedicineController> _logger;

        public PrescriptionController(IRepository repository, ILogger<MedicineController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPrescriptions()
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var prescriptions = await dbContext.Set<Prescription>().ToListAsync();

                if (prescriptions == null || prescriptions.Count == 0)
                {
                    return NotFound("No prescriptions found.");
                }

                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all prescriptions.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetPrescriptionById(uint id)
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var presciption = await dbContext.FindAsync<Prescription>(id);

                if (presciption != null)
                {
                    return Ok(presciption);
                }
                return NotFound($"Prescription with ID {id} not found.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting prescription with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> CreatePrescription([FromBody] CreatePrescriptionRequest prescription)
        {

            if (prescription.ExaminationId == null)
                return BadRequest("Examination ID cannot be null.");
            
            var examination = await _repository.GetEntityBy<uint, IExamination>((uint)prescription.ExaminationId);
            if (examination == null)
            {
                return NotFound("Can not find this examination");
            }

            var newPrescription = await examination.ObtainPrescription();
            newPrescription.Description = prescription.Description;
            newPrescription.Document = prescription.Document;

            if (!await newPrescription.Create())
                return BadRequest("Cannot create prescription.");

            return Ok(newPrescription);
        }

        [HttpPut("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> UpdatePrescription([FromBody] UpdatePrescriptionRequest prescription, uint id)
        {
            var prescriptionExist = await _repository.GetEntityBy<uint, IPrescription>(id);

            if (prescriptionExist == null)
            {
                return NotFound("Can not find this prescription");
            }

            var change = false;

            if (prescription.Document != null && prescription.Document != prescriptionExist.Document)
            {
                prescriptionExist.Document = prescription.Document;
                change = true;
            }

            if (prescription.Description != null && prescription.Description != prescriptionExist.Description)
            {
                prescriptionExist.Description = prescription.Description;
                change = true;
            }

            if (change)
            {
                if (!await prescriptionExist.Update())
                {
                    return BadRequest("Can not update prescription");
                }
                return Ok("Changed successfull");
            }
            return Ok("No change");
        }

        [HttpDelete("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> DeletePrescription(uint id)
        {
            var prescriptionExist = await _repository.GetEntityBy<uint, IPrescription>(id);

            if (prescriptionExist == null)
            {
                return NotFound("Can not find this prescription");
            }

            if (!await prescriptionExist.Delete())
            {
                return BadRequest("Can not delete this prescription");
            }

            return Ok("This prescription has been delete");
        }

    }
}