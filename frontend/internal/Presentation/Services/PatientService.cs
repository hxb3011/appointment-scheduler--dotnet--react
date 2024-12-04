using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services;

public class PatientService
{
    private readonly HttpApiService _httpApiService;
    private readonly ILogger<PatientService> _logger;

    public PatientService(HttpApiService httpApiService, ILogger<PatientService> logger)
    {
        _httpApiService = httpApiService;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientResponse>> GetPagedPatients(PagedGetAllRequest request)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");
            var jsonBody = _httpApiService.SerializeJson(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/patient", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var patients = _httpApiService.DeserializeJson<IEnumerable<PatientResponse>>(jsonString);

            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<PatientModel> GetPatientById(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"api/patient/{id}", UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var patient = _httpApiService.DeserializeJson<PatientModel>(jsonString);

            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<string> AddPatient(PatientModel patient)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var jsonBody = _httpApiService.SerializeJson(patient);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/patient", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Patient added successfully");
                return "Patient added successfully";
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
            var errorMessage = $"Error occurred while adding patient";
            _logger.LogError(e, errorMessage);
            return errorMessage;
        }
    }

    public async Task<string> UpdatePatient(PatientModel patient)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var jsonBody = _httpApiService.SerializeJson(patient);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"api/patient/{patient.Id}", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Patient updated successfully");
                return "Patient updated successfully";
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
            var errorMessage = $"Error occurred while update patient";
            _logger.LogError(e, errorMessage);
            return errorMessage;
        }
    }

	public async Task<bool> DeletePatient(uint id)
	{
		try
		{
			var token = _httpApiService.Context.Session.GetString("AuthToken");

			var httpRequest = new HttpRequestMessage
			{
				Method = HttpMethod.Delete,
				RequestUri = new Uri($"api/patient/{id}", UriKind.Relative),
			};

			if (!string.IsNullOrEmpty(token))
			{
				httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			var response = await _httpApiService.SendAsync(httpRequest);

			if (response.IsSuccessStatusCode)
			{
				_logger.LogInformation("Patient deleted successfully");
				return true;
			}
			else
			{
				_logger.LogWarning($"Failed to delete patient. Status code: {response.StatusCode}");
				return false;
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error occured while deletes patient");
			return false;
		}
	}

}