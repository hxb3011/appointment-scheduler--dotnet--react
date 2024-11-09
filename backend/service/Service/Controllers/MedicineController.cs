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
    public class MedicineController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(IRepository repository, ILogger<MedicineController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMedicine()
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var medicines = await dbContext.Set<Medicine>().ToListAsync();

                if ( medicines == null || medicines.Count == 0)
                {
                    return NotFound("No medicines found.");
                }

                return Ok(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all medicines.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetMedicineById(uint id)
        {
            try
            {
                var dbContext = await _repository.GetService<DbContext>();
                var medicine = await dbContext.FindAsync<Medicine>(id);

                if (medicine != null)
                {
                    return Ok(medicine);
                }
                return NotFound($"Medicine with ID {id} not found.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting medicine with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        [JSONWebToken(AuthenticationRequired = false)]
        public async Task<ActionResult> CreateMedicine([FromBody] CreateMedicineRequest medicine)
        {

            // Kiểm tra Name
            if (string.IsNullOrWhiteSpace(medicine.Name))
                return BadRequest("Name cannot be null or empty.");
            if (medicine.Name.Length < 5 || medicine.Name.Length > 36)
                return BadRequest("Name must be between 10 and 36 characters.");

            // Kiểm tra Unit
            if (string.IsNullOrWhiteSpace(medicine.Unit))
                return BadRequest("Unit cannot be null or empty.");

            // Tạo thuốc mới nếu các kiểm tra đã hợp lệ
            //var newMedicine = _repository.GetEntities<IMedicine>();

            //newMedicine.Name = medicine.Name;
            //newMedicine.Image = medicine.Image;
            //newMedicine.Unit = medicine.Unit;

            //if (!await newMedicine.Create())
            //return BadRequest("Cannot create profile.");

            //return Ok(newMedicine);
            return Ok();

        }

    }
}
