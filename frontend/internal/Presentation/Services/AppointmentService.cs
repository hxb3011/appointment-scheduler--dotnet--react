using AppointmentScheduler.Presentation.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AppointmentScheduler.Presentation.Services;

public class AppointmentService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AppointmentService> _logger;
    public AppointmentService(HttpClient httpClient, ILogger<AppointmentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentModel>> GetAllAppointments()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/appointment");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var appointments = JsonConvert.DeserializeObject<IEnumerable<AppointmentModel>>(jsonString);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    public async Task<AppointmentModel> GetAppointmentById(uint id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/appointment/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var appoinment = JsonConvert.DeserializeObject<AppointmentModel>(jsonString);
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
            var response = await _httpClient.PostAsJsonAsync("api/appointment", appointment);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

	public async Task<bool> UpdateAppointment(AppointmentModel appointment)
	{
		try
		{
			var response = await _httpClient.PutAsJsonAsync($"api/appointment/{appointment.Id}", appointment);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
			return false;
		}
	}

    public async Task<bool> DeleteAppointment(uint id)
    {
		try
		{
			var response = await _httpClient.DeleteAsync($"api/appointment/{id}");

			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
			return false;
		}
	}
}