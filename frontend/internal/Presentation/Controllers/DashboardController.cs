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
        AuthRequest request = new AuthRequest();
        request.Username = "root00";
        request.Password = "HeLlo|12";
        var token = await _authService.SaveTokenToService(request);

        Console.WriteLine("Token: {0}", token);
        return View();
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
