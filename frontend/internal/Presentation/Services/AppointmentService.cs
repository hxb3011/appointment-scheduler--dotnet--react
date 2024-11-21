using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services;

public class AppointmentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AppointmentService> _logger;
    public AppointmentService(HttpClient httpClient, ILogger<AppointmentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentModel>> GetAllAppointments()
    {
        try
        {
            var response = await _httpClient.GetAsync("appointment");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var appointments = JsonConvert.DeserializeObject<IEnumerable<AppointmentModel>>(jsonString);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}