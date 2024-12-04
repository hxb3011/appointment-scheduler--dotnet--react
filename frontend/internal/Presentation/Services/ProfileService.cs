using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
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

        public async Task<IEnumerable<ProfileResponse>> GetPagedProfiles(PagedGetAllRequest request)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
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
                var profiles = _httpApiService.DeserializeJson<IEnumerable<ProfileResponse>>(jsonString);

                return profiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task<ProfileModel> GetProfileById(uint id)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");

                var httpRequest = new HttpRequestMessage
                {
                    Method= HttpMethod.Get,
                    RequestUri = new Uri($"api/profile/{id}", UriKind.Relative)
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var profile = _httpApiService.DeserializeJson<ProfileModel>(jsonString);

                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

		public async Task<string> AddProfile(ProfileModel profile)
		{
			try
			{
				var token = _httpApiService.Context.Session.GetString("AuthToken");

				var jsonBody = _httpApiService.SerializeJson(profile);

				var httpRequest = new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("api/profile", UriKind.Relative),
					Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
				};

				if (!string.IsNullOrEmpty(token))
				{
					httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}

				var response = await _httpApiService.SendAsync(httpRequest);

				if (response.IsSuccessStatusCode)
				{
					_logger.LogInformation("Profile added successfully");
					return "Profile added successfully";
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
				var errorMessage = $"Error occurred while adding Profile";
				_logger.LogError(e, errorMessage);
				return errorMessage;
			}
		}

		public async Task<string> UpdateProfile(ProfileModel profile)
		{
			try
			{
				var token = _httpApiService.Context.Session.GetString("AuthToken");

				var jsonBody = _httpApiService.SerializeJson(profile);

				var httpRequest = new HttpRequestMessage
				{
					Method = HttpMethod.Put,
					RequestUri = new Uri($"api/profile/{profile.Id}", UriKind.Relative),
					Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
				};

				if (!string.IsNullOrEmpty(token))
				{
					httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}

				var response = await _httpApiService.SendAsync(httpRequest);

				if (response.IsSuccessStatusCode)
				{
					_logger.LogInformation("Profile updated successfully");
					return "Profile updated successfully";
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
				var errorMessage = $"Error occurred while update profile";
				_logger.LogError(e, errorMessage);
				return errorMessage;
			}
		}

		public async Task<bool> DeleteProfile(uint id)
		{
			try
			{
				var token = _httpApiService.Context.Session.GetString("AuthToken");

				var httpRequest = new HttpRequestMessage
				{
					Method = HttpMethod.Delete,
					RequestUri = new Uri($"api/profile/{id}", UriKind.Relative),
				};

				if (!string.IsNullOrEmpty(token))
				{
					httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
				}

				var response = await _httpApiService.SendAsync(httpRequest);

				if (response.IsSuccessStatusCode)
				{
					_logger.LogInformation("Profile deleted successfully");
					return true;
				}
				else
				{
					_logger.LogWarning($"Failed to delete profile. Status code: {response.StatusCode}");
					return false;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error occured while deletes profile");
				return false;
			}
		}
	}
}
