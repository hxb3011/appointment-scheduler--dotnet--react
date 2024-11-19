using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorDemoController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<DoctorDemoController> _logger;

        public DoctorDemoController(IRepository repository, ILogger<DoctorDemoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDoctors()
        {
            var dbContext = await _repository.GetService<DbContext>();
            var doctors = await dbContext.Set<Doctor>().ToListAsync();

            return Ok(doctors);
        }
    }
}
