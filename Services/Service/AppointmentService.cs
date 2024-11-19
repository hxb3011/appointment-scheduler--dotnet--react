using Models;
using Newtonsoft.Json;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
	public class AppointmentService : IAppointmentService
	{
		private readonly HttpClient _httpClient;

		public AppointmentService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

        public async Task<IEnumerable<AppointmentViewModel>> GetAllAppointments()
		{
			var response = await _httpClient.GetAsync("api/appointment");
			response.EnsureSuccessStatusCode();

			var jsonString = await response.Content.ReadAsStringAsync();
			var appointments = JsonConvert.DeserializeObject<IEnumerable<AppointmentViewModel>>(jsonString);

			return appointments;
        }

        public async Task<bool> CreateAppointment(AppointmentViewModel appointment)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/appointment", appointment);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while adding appointment: " + ex.Message);
                return false;
            }
        }
    }
}
