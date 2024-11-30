using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AppointmentScheduler.Presentation.Services;

public class AppointmentService
{
    private readonly HttpApiService _httpApiService;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(HttpApiService httpApiService, ILogger<AppointmentService> logger)
    {
        _httpApiService = httpApiService;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentModel>> GetPagedAppointmentsWithBodyAsync(PagedGetAllRequest request)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");
            var jsonBody = _httpApiService.SerializeJson(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/appointment", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var appointments = _httpApiService.DeserializeJson<IEnumerable<AppointmentModel>>(jsonString);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching appointments.");
            return null;
        }
    }



    public async Task<AppointmentModel> GetAppointmentById(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method= HttpMethod.Get,
                RequestUri = new Uri($"api/appointment/{id}", UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var appoinment = _httpApiService.DeserializeJson<AppointmentModel>(jsonString);
                return appoinment;
            }
            else
            {
                Console.WriteLine("Failed to retrieve appointment. Status code: " + response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while getting appointmnent: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> AddAppointment(AppointmentModel appointment)
    {
        try
        {
            // Lấy token từ session
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            // Serialize model sang JSON
            var jsonBody = _httpApiService.SerializeJson(appointment);

            // Tạo HTTP request
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/appointment", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            // Thêm token vào header nếu có
            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gửi request
            var response = await _httpApiService.SendAsync(httpRequest);

            // Kiểm tra trạng thái phản hồi
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Appointment added successfully.");
                return true;
            }
            else
            {
                _logger.LogWarning($"Failed to add appointment. Status code: {response.StatusCode}");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding appointment.");
            return false;
        }
    }


    //public async Task<bool> UpdateAppointment(AppointmentModel appointment)
    //{
    //	try
    //	{
    //		var response = await _httpClient.PutAsJsonAsync($"api/appointment/{appointment.Id}", appointment);

    //		if (response.IsSuccessStatusCode)
    //		{
    //			return true;
    //		}
    //		return false;
    //	}
    //	catch (Exception ex)
    //	{
    //		_logger.LogError(ex.Message);
    //		return false;
    //	}
    //}

    //   public async Task<bool> DeleteAppointment(uint id)
    //   {
    //	try
    //	{
    //		var response = await _httpClient.DeleteAsync($"api/appointment/{id}");

    //		if (response.IsSuccessStatusCode)
    //		{
    //			return true;
    //		}
    //		return false;
    //	}
    //	catch (Exception ex)
    //	{
    //		_logger.LogError(ex.Message);
    //		return false;
    //	}
    //}
}