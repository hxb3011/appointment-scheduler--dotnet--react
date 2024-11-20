#define DEMO

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentScheduler.Infrastructure.Authorization;

public class JSONWebTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRepository _repository;
    private readonly JSONWebTokenOptions _options;

    public JSONWebTokenMiddleware(RequestDelegate next, IRepository repository, JSONWebTokenOptions options)
    {
        _next = next;
        _repository = repository;
        _options = options;
    }
    private static IEnumerable<string> ValueOfType(IEnumerable<Claim> claims, string type)
    {
        return claims.AsQueryable().Where(c => c.Type == type).Select(c => c.Value);
    }
    public async Task Invoke(HttpContext context)
    {
        var attr = context.GetEndpoint()?.Metadata.GetMetadata<JSONWebTokenAttribute>();
        if (attr != null && attr.AuthenticationRequired)
        {
            var auth = context.Request.Headers["Authorization"].FirstOrDefault();
            if (auth == null) goto unauthorization;
            var authInfo = auth.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string token;
            if (authInfo.Length != 2
                || authInfo.First().Equals("bearer", StringComparison.InvariantCultureIgnoreCase)
                || (token = authInfo.Last()) == null) goto unauthorization;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SymmetricSecurityKey));
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = key
            }, out SecurityToken validateToken);
            var jwtToken = (JwtSecurityToken)validateToken;
            var user = await _repository.GetEntityBy<string, IUser>(ValueOfType(jwtToken.Claims, JSONWebTokenOptions.Id).FirstOrDefault());
            if (user == null) goto unauthorization;

            if (!attr.RequiredPermissions.Select(p => Enum.GetName(p)).ToHashSet().IsSubsetOf(ValueOfType(jwtToken.Claims, JSONWebTokenOptions.Permissions)))
            {
                Console.WriteLine("Required: {0}", attr.RequiredPermissions.Select(p => Enum.GetName(p)).Aggregate((a, k) => a + ", " + k));
                Console.WriteLine("Supplied: {0}", ValueOfType(jwtToken.Claims, JSONWebTokenOptions.Permissions).Aggregate((a, k) => a + ", " + k));
                var response = context.Response;
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new JsonObject
                {
                    ["message"] = "permissions denied"
                });
                return;
            }
            context.SetAuthUser(user);
        }
        await _next(context);
        return;

    unauthorization:
        {
            var response = context.Response;
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new JsonObject
            {
                ["message"] = "unauthorization"
            });
            return;
        }
    }
}
