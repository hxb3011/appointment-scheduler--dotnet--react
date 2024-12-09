using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Responses;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class ScheduleService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(HttpApiService httpApiService, ILogger<ScheduleService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        // Get all scheduler parts
        public async Task<IEnumerable<SchedulerPart>> GetAll()
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/scheduler", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var schedulerParts = _httpApiService.DeserializeJson<IEnumerable<SchedulerPart>>(jsonString);

                return schedulerParts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetPartsAsync: {ex.Message}");
                return null;
            }
        }

        // Get schedule by start time
        public async Task<SchedulerPart> GetScheduleById(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/scheduler/{id}", UriKind.Relative) // Format TimeOnly as HH:mm
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var schedulerPart = _httpApiService.DeserializeJson<SchedulerPart>(jsonString);

                return schedulerPart;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetSchedule: {ex.Message}");
                return null;
            }
        }

    }
}
