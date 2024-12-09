using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services
{
    public class RoleService
    {
        private readonly HttpApiService _httpApiService;
        private readonly ILogger<RoleService> _logger;

        public RoleService(HttpApiService httpApiService, ILogger<RoleService> logger)
        {
            _httpApiService = httpApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleResponse>> GetPagedRoles(PagedGetAllRequest request)
        {
            try
            {
                var token = _httpApiService.Context.Session.GetString("AuthToken");
                var jsonBody = _httpApiService.SerializeJson(request);

                var httpRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/role", UriKind.Relative),
                    Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
                };

                if (!string.IsNullOrEmpty(token))
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpApiService.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var roles = _httpApiService.DeserializeJson<IEnumerable<RoleResponse>>(jsonString);

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
