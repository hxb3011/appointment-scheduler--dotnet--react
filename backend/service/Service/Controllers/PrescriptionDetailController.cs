using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionDetailController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<MedicineController> _logger;

        public PrescriptionDetailController(IRepository repository, ILogger<MedicineController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPrescriptionDetails()
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var prescriptionDetails = await dbContext.Set<PrescriptionDetail>().ToListAsync();

                if (prescriptionDetails == null || prescriptionDetails.Count == 0)
                {
                    return NotFound("No prescription details found.");
                }

                return Ok(prescriptionDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all prescription details.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetPrescriptionDetailById(uint id)
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var presciptionDetail = await dbContext.FindAsync<PrescriptionDetail>(id);

                if (presciptionDetail != null)
                {
                    return Ok(presciptionDetail);
                }
                return NotFound($"Prescription detail with ID {id} not found.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting prescription detail with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> CreatePrescriptionDetail([FromBody] CreatePrescriptionDetailRequest prescriptionDetail)
        {

            if (prescriptionDetail.PrescriptionId == null)
                return BadRequest("Prescription ID cannot be null.");

            var prescription = await _repository.GetEntityBy<uint, IPrescription>((uint)prescriptionDetail.PrescriptionId);
            if (prescription == null)
            {
                return NotFound("Can not find this prescription");
            }


            if (prescriptionDetail.MedicineId == null)
                return BadRequest("Medicine ID cannot be null.");
            var medicine = await _repository.GetEntityBy<uint, IMedicine>((uint)prescriptionDetail.MedicineId);
            if (prescription == null)
            {
                return NotFound("Can not find this medicine");
            }

            var newPrescriptionDetail = await prescription.ObtainPrescriptionDetail();
            newPrescriptionDetail.Medicine = medicine;
            newPrescriptionDetail.Description = prescriptionDetail.Description;

            if (!await newPrescriptionDetail.Create())
                return BadRequest("Cannot create prescription detail.");

            return Ok(newPrescriptionDetail);
        }

        [HttpPut("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> UpdatePrescription([FromBody] UpdatePrescriptionDetailRequest prescriptionDetail, uint id)
        {
            var prescriptionDetailExist = await _repository.GetEntityBy<uint, IPrescriptionDetail>(id);

            if (prescriptionDetailExist == null)
            {
                return NotFound("Can not find this prescription detail");
            }

            var change = false;

            if (prescriptionDetail.Description != null && prescriptionDetail.Description != prescriptionDetailExist.Description)
            {
                prescriptionDetailExist.Description = prescriptionDetail.Description;
                change = true;
            }

            if (change)
            {
                if (!await prescriptionDetailExist.Update())
                {
                    return BadRequest("Can not update prescription detail");
                }
                return Ok("Changed successfull");
            }
            return Ok("No change");
        }

        [HttpDelete("{id}")]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> DeletePrescriptionDetail(uint id)
        {
            var prescriptionDetailExist = await _repository.GetEntityBy<uint, IPrescriptionDetail>(id);

            if (prescriptionDetailExist == null)
            {
                return NotFound("Can not find this prescription detail");
            }

            if (!await prescriptionDetailExist.Delete())
            {
                return BadRequest("Can not delete this prescription detail");
            }

            return Ok("This prescription detail has been delete");
        }
    }
}
