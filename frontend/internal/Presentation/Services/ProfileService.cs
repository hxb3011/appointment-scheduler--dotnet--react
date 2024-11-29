using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class ProfileService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<ProfileService> _logger;
        public ProfileService(HttpApiService httpApiService, ILogger<ProfileService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<Profile>> GetPagedProfiles(PagedGetAllRequest request, string token)
        {
            try
            {
                var jsonBody = _httpApiService.SerializeJson(request);
                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/profile", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
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
    }

    //    public async Task<Profile> GetProfileById(uint id)
    //    {
    //        try
    //        {
    //            var response = await _httpClient.GetAsync("api/profile/" + id);
    //            response.EnsureSuccessStatusCode();

    //            var jsonString = await response.Content.ReadAsStringAsync();
    //            var profile = JsonConvert.DeserializeObject<Profile>(jsonString);

    //            return profile;
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex.Message);
    //            return null;
    //        }
    //    }
    //}
}
