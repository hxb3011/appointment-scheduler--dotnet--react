using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProfileInfo(uint id)
        {
            var profile = await _profileService.GetProfileById(id);
            return Ok(profile);
        }
    }
}
