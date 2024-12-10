using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Presentation.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace AppointmentScheduler.Presentation.Services;

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

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"api/role?offset={request.Offset}&count={request.Count}" + (request.By == null ? "" : "&by=" + request.By), UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await _httpApiService
                .DeserializeAsyncJson<IEnumerable<RoleResponse>>(
                    await response.Content.ReadAsStreamAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<RoleResponse> GetRoleById(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/role/" + id.ToString(), UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await _httpApiService
                .DeserializeAsyncJson<RoleResponse>(
                    await response.Content.ReadAsStreamAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<string> CreateRole(RoleRequest request)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");
            var jsonBody = _httpApiService.SerializeJson(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("api/role", UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
    public async Task<bool> CheckDefaultRole(uint id) 
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/role/default/" + id, UriKind.Relative),
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return "success" == await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateDefaultRole(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("api/role/default/" + id, UriKind.Relative),
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return "success" == await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<string> UpdateRole(RoleRequest request, uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");
            var jsonBody = _httpApiService.SerializeJson(request);

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("api/role/" + id, UriKind.Relative),
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<string> DeleteRole(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("api/role/" + id, UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    private static Permission AsPermission(string permid)
        => Enum.TryParse(permid, out Permission permission) ? permission
            : (Permission)Enum.GetNames<Permission>().Length;

    public async Task<IEnumerable<Permission>> GetPermissions()
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/role/permissions", UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return (await _httpApiService
                .DeserializeAsyncJson<IEnumerable<string>>(
                    await response.Content.ReadAsStreamAsync()))
                .Select(AsPermission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Permission>> GetPermissions(uint id)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"api/role/{id}/permissions", UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return (await _httpApiService
                .DeserializeAsyncJson<IEnumerable<string>>(
                    await response.Content.ReadAsStreamAsync()))
                .Select(AsPermission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public async Task<string> ChangePermission(uint id, Permission perm, bool granted)
    {
        try
        {
            var token = _httpApiService.Context.Session.GetString("AuthToken");

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"api/role/{id}/permission/{Enum.GetName(perm)}?granted={granted}", UriKind.Relative)
            };

            if (!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpApiService.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}