using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IAppointmentService> _logger;

        public AppointmentService(HttpClient httpClient, ILogger<IAppointmentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAllAppointments()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/appointment");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<IEnumerable<AppointmentViewModel>>(jsonString);

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
}
