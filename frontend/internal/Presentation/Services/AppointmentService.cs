using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
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

    public async Task<IEnumerable<AppointmentModel>> GetPagedAppointmentsWithBodyAsync(PagedGetAllRequest request, string token)
    {
        try
        {
            // Serialize body từ request
            var jsonBody = _httpApiService.SerializeJson(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/appointment", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            // Thêm Bearer token vào header Authorization
            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Gửi yêu cầu sử dụng HttpApiService
            var response = await _httpApiService.SendAsync(httpRequest);

            // Kiểm tra phản hồi
            response.EnsureSuccessStatusCode();

            // Deserialize response
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



    //   public async Task<AppointmentModel> GetAppointmentById(uint id)
    //   {
    //       try
    //       {
    //           var response = await _httpClient.GetAsync($"api/appointment/{id}");
    //           if (response.IsSuccessStatusCode)
    //           {
    //               var jsonString = await response.Content.ReadAsStringAsync();
    //               var appoinment = JsonConvert.DeserializeObject<AppointmentModel>(jsonString);
    //               return appoinment;
    //           }
    //           else
    //           {
    //               Console.WriteLine("Failed to retrieve appointment. Status code: " + response.StatusCode);
    //               return null;
    //           }
    //       }
    //       catch (Exception ex)
    //       {
    //           Console.WriteLine("An error occurred while getting appointmnent: " + ex.Message);
    //           return null;
    //       }
    //   }

    //   public async Task<bool> AddAppointment(AppointmentModel appointment)
    //   {
    //       try
    //       {
    //           var response = await _httpClient.PostAsJsonAsync("api/appointment", appointment);

    //           if (response.IsSuccessStatusCode)
    //           {
    //               return true;
    //           }
    //           return false;
    //       }catch (Exception ex)
    //       {
    //           _logger.LogError(ex.Message);
    //           return false;
    //       }
    //   }

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