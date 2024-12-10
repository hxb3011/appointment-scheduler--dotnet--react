using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class ExaminationService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<ExaminationService> _logger;
        public ExaminationService(HttpApiService httpApiService, ILogger<ExaminationService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<ExaminationModel>> GetPagedExamination(PagedGetAllRequest request)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
                var jsonBody = _httpApiService.SerializeJson(request);
                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/examination", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var examinations = _httpApiService.DeserializeJson<IEnumerable<ExaminationModel>>(jsonString);

                return examinations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<ExaminationModel> GetExaminationById(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/examination/{id}", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var examination = _httpApiService.DeserializeJson<ExaminationModel>(jsonString);

                return examination;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> ExistExaminationByAppointment(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/examination/appointment/{id}", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> AddExamination(uint appointmentId)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Authorization token is missing.");
                    return "Authorization token is missing.";
                }

                var requestUri = new Uri($"api/examination/{appointmentId}", UriKind.Relative);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = requestUri
                };

                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Examination created successfully for appointment {appointmentId}", appointmentId);
                    return "Examination added successfully";
                }
                else
                {
                    var errorMessage = "Failed to create examination.";
                    var errorContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        errorMessage += $" Error: {errorContent}";
                    }

                    _logger.LogWarning(errorMessage);
                    return errorMessage;
                }
            }
            catch (Exception e)
            {
                var errorMessage = $"Error occurred while creating examination for appointment {appointmentId}";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }


        public async Task<string> UpdateExamination(ExaminationModel exam)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(exam);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"api/examination/{exam.Id}", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Exam updated successfully");
                    return "Examination updated successfully";
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
                var errorMessage = $"Error occurred while update examination";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }

        public async Task<bool> DeleteExamination(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"api/examination/{id}", UriKind.Relative),
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Examination deleted successfully");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to delete examination. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while deletes examination");
                return false;
            }
        }


        public async Task<PrescriptionResponse> GetPrescription(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"api/examination/{id}/perscription", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                var response = await _httpApiService.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var perscription = _httpApiService.DeserializeJson<PrescriptionResponse>(jsonString);

                    return perscription;
                }
                else
                {
                    return null;
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<string> AddPrescription(uint examId, PrescriptionRequest prescription)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var jsonBody = _httpApiService.SerializeJson(prescription);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"api/examination/{examId}/perscription", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Perscription added successfully");
                    return "Perscription added successfully";
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
                var errorMessage = $"Error occurred while adding perscription";
                _logger.LogError(e, errorMessage);
                return errorMessage;
            }
        }

        public async Task<bool> DeletePerscription(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"api/examination/{id}/perscription", UriKind.Relative),
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Perscription deleted successfully");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"Failed to delete perscription. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while deletes perscription");
                return false;
            }
        }
    }
}
