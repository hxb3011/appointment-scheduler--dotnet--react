using AppointmentScheduler.Domain.Business;
using Microsoft.AspNetCore.Http;

namespace AppointmentScheduler.Infrastructure.Authorization;

public static class JSONWebTokenExtensions
{
    public static IUser GetAuthUser(this HttpContext context)
    {
        if (context.Items.TryGetValue("user", out var o) && o is IUser user)
            return user;

        throw new InvalidOperationException("Authentication Required.");
    }
    internal static void SetAuthUser(this HttpContext context, IUser user)
    {
        context.Items["user"] = user;
    }
}