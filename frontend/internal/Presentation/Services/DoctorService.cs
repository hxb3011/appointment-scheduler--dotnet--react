using AppointmentScheduler.Domain.Entities;
using Newtonsoft.Json;

namespace AppointmentScheduler.Presentation.Services
{
    public class DoctorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DoctorService> _logger;
        public DoctorService(HttpClient httpClient, ILogger<DoctorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/doctor");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var doctors = JsonConvert.DeserializeObject<IEnumerable<Doctor>>(jsonString);

                return doctors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Doctor> GetDoctorById(uint id)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/doctor/" + id);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var doctor = JsonConvert.DeserializeObject<Doctor>(jsonString);

                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }

}
