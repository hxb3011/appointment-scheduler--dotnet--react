using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models;
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

        public async Task<IEnumerable<DoctorModel>> GetPagedDoctors(PagedGetAllRequest request)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
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
                var doctors = _httpApiService.DeserializeJson<IEnumerable<DoctorModel>>(jsonString);

                return doctors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<DoctorModel> GetDoctorById(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/doctor/{id}", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var doctor = _httpApiService.DeserializeJson<DoctorModel>(jsonString);

                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<string> AddDoctor(DoctorModel doctor)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(doctor);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("api/doctor", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Doctor added successfully");
                    return "Doctor added successfully";
                }
                else
                {
                    var errorMessage = "";

                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        errorMessage += $"Error: {errorContent}";
                    }

                    _logger.LogWarning(errorMessage);
                    return errorMessage; 
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"Error occurred while adding doctor";
                _logger.LogError(e, errorMessage);
                return errorMessage; 
            }
        }



        public async Task<string> UpdateDoctor(DoctorModel doctor)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(doctor);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"api/doctor/{doctor.Id}", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Doctor updated successfully");
                    return "Doctor updated successfully";
                }
                else
                {
                    var errorMessage = "";

                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        errorMessage += $"Error: {errorContent}";
                    }

                    _logger.LogWarning(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"Error occurred while update Doctor";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }

        public async Task<bool> DeleteDoctor(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"api/doctor/{id}", UriKind.Relative),
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Doctor deleted successfully");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to delete doctor. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while deletes doctor");
                return false;
            }
        }
    }

}
