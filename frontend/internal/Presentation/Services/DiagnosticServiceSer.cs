using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services
{
    public class DiagnosticServiceSer
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DiagnosticServiceSer> _logger;

        public DiagnosticServiceSer(HttpClient httpClient, ILogger<DiagnosticServiceSer> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticService>> GetAllDiagnosticService()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/diagnosticService");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var diagnosticServices = JsonConvert.DeserializeObject<IEnumerable<DiagnosticService>>(jsonString);

                return diagnosticServices;
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
