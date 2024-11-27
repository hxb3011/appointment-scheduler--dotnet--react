using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services
{
    public class DiagnosticService
    {
        private readonly HttpClient _client;
        private readonly ILogger<DiagnosticService> _logger;
        public DiagnosticService(HttpClient client, ILogger<DiagnosticService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IEnumerable<Domain.Entities.DiagnosticService>> GetAllDiagnosticService()
        {
            try
            {
                var response = await _client.GetAsync("api/diagnosticService");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var diagnosticServices = JsonConvert.DeserializeObject<IEnumerable<Domain.Entities.DiagnosticService>>(jsonString);

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
