#define DEMO

using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentScheduler.Infrastructure.Authorization;

public class JSONWebTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IRepository _repository;
    private readonly JSONWebTokenOptions _options;
    private readonly ILogger<JSONWebTokenMiddleware> _logger;

    public JSONWebTokenMiddleware(RequestDelegate next, IRepository repository, JSONWebTokenOptions options, ILogger<JSONWebTokenMiddleware> logger)
    {
        _next = next;
        _repository = repository;
        _options = options;
        _logger = logger;
    }
    private static IEnumerable<string> ValueOfType(IEnumerable<Claim> claims, string type)
    {
        var param = Expression.Parameter(typeof(Claim));
        return claims.AsQueryable().Where(Expression.Lambda<Func<Claim, bool>>(
            Expression.Equal(
                Expression.Property(param, nameof(Claim.Type)),
                Expression.Constant(type)
            ), param
        )).Select(Expression.Lambda<Func<Claim, string>>(
            Expression.Property(param, nameof(Claim.Value)), param
        ));
    }
    public async Task Invoke(HttpContext context)
    {
        var attr = context.GetEndpoint()?.Metadata.GetMetadata<JSONWebTokenAttribute>();
        if (attr != null && attr.AuthenticationRequired)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                await ErrorResponse(context.Response);
                return;
            }
            _logger.LogInformation("(Authorization: {0})", token);

            var authInfo = token.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (authInfo.Length != 2 || !"bearer".Equals(authInfo.First(),
                    StringComparison.InvariantCultureIgnoreCase)
                || (token = authInfo.Last()) == null)
            {
                await ErrorResponse(context.Response);
                return;
            }
            _logger.LogInformation("(Token: {0})", token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateAudience = false, ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.SymmetricSecurityKey)
                )
            };

            JwtSecurityToken jwtToken;
            try
            {
                tokenHandler.ValidateToken(token, parameters, out SecurityToken validateToken);
                jwtToken = (JwtSecurityToken)validateToken;
            }
            catch (SecurityTokenExpiredException)
            {
                await ErrorResponse(context.Response, message: "access token expired");
                return;
            }

            var user = await _repository.GetEntityBy<string, IUser>(ValueOfType(jwtToken.Claims, JSONWebTokenOptions.Id).FirstOrDefault());
            if (user == null)
            {
                await ErrorResponse(context.Response);
                return;
            }

            var requiredPermissionNames = attr.RequiredPermissions.Select(Enum.GetName);
            var suppliedPermissionNames = ValueOfType(jwtToken.Claims, JSONWebTokenOptions.Permissions);
            if (!requiredPermissionNames.ToHashSet().IsSubsetOf(suppliedPermissionNames))
            {
                Console.WriteLine("{{\n\t\"required\": [\n\t\t\"{0}\"\n\t],\n\t\"supplied\": [\n\t\t\"{1}\"\n\t]\n}}",
                    string.Join("\",\n\t\t\"", requiredPermissionNames), string.Join("\",\n\t\t\"", suppliedPermissionNames));
                await ErrorResponse(context.Response, StatusCodes.Status403Forbidden, "permissions denied");
                return;
            }

            context.SetAuthUser(user);
        }
        await _next(context);
        return;
    }

    private static Task ErrorResponse(HttpResponse response,
        int statusCode = StatusCodes.Status401Unauthorized,
        string message = "unauthorization")
    {
        response.StatusCode = statusCode;
        response.ContentType = "application/json";
        return response.WriteAsJsonAsync(new JsonObject { ["message"] = message });
    }
}
