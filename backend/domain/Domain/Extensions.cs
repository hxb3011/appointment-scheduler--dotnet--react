using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Domain;

public static class Extensions
{
    public static IEnumerable<Permission> Exclude(this IEnumerable<Permission> value, params Permission[] permissions)
    {
        if (permissions == null) return value;
        var it = permissions.GetEnumerator();
        if (!it.MoveNext()) return value;
        var pE = Expression.Parameter(typeof(Permission));
        var body = Expression.NotEqual(pE, Expression.Constant(it.Current, typeof(Permission)));
        while (it.MoveNext())
            body = Expression.AndAlso(body,
                Expression.NotEqual(pE, Expression.Constant(it.Current, typeof(Permission))));
        return value.Where(Expression.Lambda<Func<Permission, bool>>(body, pE).Compile());
    }

    public static void GrantTo(this IEnumerable<Permission> value, IRole role)
    {
        foreach (var perm in value) role.SetPermissionGranted(perm, true);
    }

    public static void DenyFrom(this IEnumerable<Permission> value, IRole role)
    {
        foreach (var perm in value) role.SetPermissionGranted(perm, false);
    }

    public static string Env(this string name)
         => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)
            ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User)
            ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
}
