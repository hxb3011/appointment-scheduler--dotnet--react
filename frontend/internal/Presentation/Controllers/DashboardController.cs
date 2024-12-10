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

    public DashboardController(ILogger<DashboardController> logger, AuthService authService)
    {
        _authService = authService;
        _logger = logger;
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
