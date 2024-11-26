using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services
{
    public class ProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProfileService> _logger;
        public ProfileService(HttpClient httpClient, ILogger<ProfileService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Profile>> GetAllProfiles()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/profile");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var profiles = JsonConvert.DeserializeObject<IEnumerable<Profile>>(jsonString);

                return profiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<Profile> GetProfileById(uint id)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/profile/" + id);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var profile = JsonConvert.DeserializeObject<Profile>(jsonString);

                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
