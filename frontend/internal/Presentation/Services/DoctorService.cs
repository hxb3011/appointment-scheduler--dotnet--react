using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class DoctorService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<DoctorService> _logger;
        public DoctorService(HttpApiService httpApiService, ILogger<DoctorService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;

        }

        public async Task<IEnumerable<Doctor>> GetPagedDoctors(PagedGetAllRequest request, string token)
        {
            try
            {
                var jsonBody = _httpApiService.SerializeJson(request);

                var httpRequest = new HttpRequestMessage{
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/doctor", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var doctors = _httpApiService.DeserializeJson<IEnumerable<Doctor>>(jsonString);

                return doctors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        //public async Task<Doctor> GetDoctorById(uint id)
        //{
        //    try
        //    {
        //        var response = await _httpClient.GetAsync("api/doctor/" + id);
        //        response.EnsureSuccessStatusCode();

        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        var doctor = JsonConvert.DeserializeObject<Doctor>(jsonString);

        //        return doctor;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return null;
        //    }
        //}
    }

}
