using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class DiagnosticSerService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<DiagnosticSerService> _logger;
        public DiagnosticSerService(HttpApiService httpApiService, ILogger<DiagnosticSerService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticServiceResponse>> GetPagedDiagnosticSers(PagedGetAllRequest request)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
                var jsonBody = _httpApiService.SerializeJson(request);
                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/diagnosticService", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var diagnosticSers = _httpApiService.DeserializeJson<IEnumerable<DiagnosticServiceResponse>>(jsonString);

                return diagnosticSers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<DiagnosticServiceResponse> GetDiagnosticSerById(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/diagnosticService/{id}", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var diagnosticSer = _httpApiService.DeserializeJson<DiagnosticServiceResponse>(jsonString);

                return diagnosticSer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<string> AddDiagnosticService(DiagnosticSerModel diagnostic)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(diagnostic);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("api/diagnosticService", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Diagnostic added successfully");
                    return "Diagnostic added successfully";
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
                var errorMessage = $"Error occurred while adding Diagnostic";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }

        public async Task<string> UpdateDiagnosticService(DiagnosticSerModel diagnostic)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(diagnostic);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"api/diagnosticService/{diagnostic.Id}", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Diagnostic updated successfully");
                    return "Diagnostic updated successfully";
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
                var errorMessage = $"Error occurred while update diagnostic";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }

        public async Task<bool> DeleteDiagnosticService(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"api/diagnosticService/{id}", UriKind.Relative),
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Diagnostic deleted successfully");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to delete diagnostic. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while deletes diagnostic");
                return false;
            }
        }
    }
}
