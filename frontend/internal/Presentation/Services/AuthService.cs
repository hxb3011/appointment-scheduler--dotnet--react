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
					RequestUri = new Uri("api/auth/token/doctor", UriKind.Relative),
					Content = new FormUrlEncodedContent(new Dictionary<string, string>
					{
						{ "Username", request.Username },
						{ "Password", request.Password }
					})
				});

				// Ensure the response indicates success
				if (response.IsSuccessStatusCode)
				{
					// Deserialize response content to get the token
					var jsonString = await response.Content.ReadAsStringAsync();
					var authResponse = JsonSerializer.Deserialize<AuthTokenResponse>(jsonString);

					return authResponse?.AccessToken ?? throw new InvalidOperationException("Token not found in the response.");
				}
				else
				{
					return null;
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<string> SaveTokenToService(AuthRequest request)
		{
			var token = await Token(request);

			if(token == null)
			{
				return null;
			}
			_httpApiService.Context.Session.SetString("AuthToken", token);
			return token;
		}
	}
}
