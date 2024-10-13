using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class JSONWebTokenAttribute : Attribute
{
    private Permission[]? _requiredPermissions;
    private bool _authenticationRequired;

    public JSONWebTokenAttribute() { }

    public Permission[] RequiredPermissions { get => _requiredPermissions ?? []; set => _requiredPermissions = value; }
    public bool AuthenticationRequired
    {
        get => RequiredPermissions.LongLength != 0 || _authenticationRequired;
        set => _authenticationRequired = value;
    }
}
