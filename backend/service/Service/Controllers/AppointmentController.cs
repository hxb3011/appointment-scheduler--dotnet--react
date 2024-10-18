using AppointmentScheduler.Domain.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<ActionResult> GetAppointmentById(int id)
        {
            return null;
        }
    }
}
