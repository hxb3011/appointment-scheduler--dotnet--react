using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using System.Text.Json;

namespace AppointmentScheduler.Presentation.Services
{
	public class AuthService
	{
		private readonly HttpApiService _httpApiService;

		public AuthService(HttpApiService httpApiService)
		{
			_httpApiService = httpApiService;
		}

		public async Task<string> Token(AuthRequest request)
		{
			try
			{
				// Send POST request to API
				var response = await _httpApiService.SendAsync(new HttpRequestMessage
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("api/auth/token", UriKind.Relative),
					Content = new FormUrlEncodedContent(new Dictionary<string, string>
					{
						{ "Username", request.Username },
						{ "Password", request.Password }
					})
				});

				// Ensure the response indicates success
				response.EnsureSuccessStatusCode();

				// Deserialize response content to get the token
				var jsonString = await response.Content.ReadAsStringAsync();
				var authResponse = JsonSerializer.Deserialize<AuthTokenResponse>(jsonString);

				return authResponse?.AccessToken ?? throw new InvalidOperationException("Token not found in the response.");
			}
			catch (HttpRequestException ex)
			{
				// Log error or handle specific HTTP errors
				throw new Exception($"HTTP Request Error: {ex.Message}");
			}
			catch (Exception ex)
			{
				// Log error or rethrow for further handling
				throw new Exception($"An error occurred while retrieving the token: {ex.Message}");
			}
		}
	}
}
