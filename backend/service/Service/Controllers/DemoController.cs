//#define DEMO

//using AppointmentScheduler.Domain.Business;
//using AppointmentScheduler.Domain.Entities;
//using AppointmentScheduler.Domain.Repositories;
//using AppointmentScheduler.Infrastructure.Authorization;
//using AppointmentScheduler.Infrastructure.Business;
//using AppointmentScheduler.Infrastructure.Repositories;
//using Microsoft.AspNetCore.Mvc;

//namespace AppointmentScheduler.Service.Controllers;

//public record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}

//public class AuthRequest
//{
//    public string Email { get; set; }
//    public string Password { get; set; }
//}

//public class AuthResponse
//{
//    public string Email { get; set; }
//    public string Token { get; set; }
//}

//[ApiController]
//[Route("/[controller]")]
//[JSONWebToken(RequiredPermissions = [Permission.Perm1])]
//public class DemoControllerJWTController(IRepository repository, ILogger<DemoControllerJWTController> logger, JSONWebTokenOptions options) : ControllerBase
//{
//    private readonly IRepository _repository = repository;
//    private readonly ILogger<DemoControllerJWTController> _logger = logger;
//    private readonly JSONWebTokenOptions _options = options;

//    [HttpPost]
//    [Route("login")]
//    [JSONWebToken(AuthenticationRequired = false)] // Override Controller Rule
//    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }

//        if (request.Email != "test@company.com")
//            return BadRequest("Bad credentials");

//        // var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

//        // if (userInDb is null)
//        // {
//        //     return Unauthorized();
//        // }

//        // var accessToken = _tokenService.CreateToken(userInDb);
//        // await _context.SaveChangesAsync();

//        return Ok(new AuthResponse
//        {
//            Token = _options.GetJSONWebToken(new DemoUserImpl(), _repository)!,
//        });
//    }

//    [HttpGet]
//    [Route("demo")] // authentication and perm1 will be check
//    public async Task<ActionResult<AuthResponse>> Demo()
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }
//        var user = HttpContext.GetAuthUser();
//        return Ok(new AuthResponse
//        {
//            Token = "accessToken" + user.UserName,
//        });
//    }
//}

//[ApiController]
//[Route("/[controller]")]
//public class DemoActionJWTController(IRepository repository, ILogger<DemoActionJWTController> logger, JSONWebTokenOptions options) : ControllerBase
//{
//    private readonly IRepository _repository = repository;
//    private readonly ILogger<DemoActionJWTController> _logger = logger;
//    private readonly JSONWebTokenOptions _options = options;

//    [HttpPost]
//    [Route("login")]
//    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
//    {
//        if (!ModelState.IsValid)
//        {
//            _logger.LogError("ModelState.IsNotValid");
//            return BadRequest(ModelState);
//        }

//        if (!"test@company.com".Equals(request.Email))
//        {
//            _logger.LogError("Email.IsNotValid: " + request.Email + " " + request.Password);
//            return BadRequest("Bad credentials");
//        }

//        // var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

//        // if (userInDb is null)
//        // {
//        //     return Unauthorized();
//        // }

//        // var accessToken = _tokenService.CreateToken(userInDb);
//        // await _context.SaveChangesAsync();

//        return Ok(new AuthResponse
//        {
//            Token = _options.GetJSONWebToken(new DemoUserImpl(), _repository)!,
//        });
//    }

//    [HttpGet]
//    [Route("demo")]
//    [JSONWebToken(RequiredPermissions = [Permission.Perm2, Permission.Perm3])] // authentication, perm2 and perm3 will be check
//    public ActionResult<AuthResponse> GetDemo()
//    {
//        if (!ModelState.IsValid)
//        {
//            return BadRequest(ModelState);
//        }
//        var user = HttpContext.GetAuthUser();
//        return Ok(new AuthResponse
//        {
//            Token = "accessToken" + user.UserName,
//        });
//    }

//    private static readonly string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

//    [HttpGet]
//    [Route("weatherforecast")]
//    public WeatherForecast[] GetWeatherForecast()
//    {
//        var forecast = Enumerable.Range(1, 5).Select(index =>
//            new WeatherForecast
//            (
//                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                Random.Shared.Next(-20, 55),
//                summaries[Random.Shared.Next(summaries.Length)]
//            ))
//            .ToArray();
//        return forecast;
//    }
//}