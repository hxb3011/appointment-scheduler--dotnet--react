using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Services;
using AppointmentScheduler.Domain.Requests;

namespace AppointmentScheduler.Presentation.Controllers;

public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;
    private readonly AuthService _authService;
    private readonly DoctorService _doctorService;
    private readonly RoleService _roleService;
    private readonly HttpApiService _httpApiService;

    public DashboardController(
        ILogger<DashboardController> logger, 
        AuthService authService,
        DoctorService doctorService,
        RoleService roleService,
        HttpApiService httpApiService)
    {
        _authService = authService;
        _logger = logger;
        _doctorService = doctorService;
        _roleService = roleService;
        _httpApiService = httpApiService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var token = await _authService.SaveTokenToService(request);
        if(token != null)
        {
			var user = await _doctorService.GetCurrentDoctor();
            var role = await _roleService.GetRoleById(user.RoleId);

            var userLogin = new UserLoginModel();

            userLogin.Id = user.Id;
            userLogin.FullName = user.FullName;
            userLogin.Role = role.Name;

            var userLoginJson = _httpApiService.SerializeJson(userLogin);

            _httpApiService.Context.Session.SetString("user", userLoginJson);

			return RedirectToAction(nameof(Index));
        }

        return View(request);
    }

    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("AuthToken");
        return RedirectToAction(nameof(Login));
    }

    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
