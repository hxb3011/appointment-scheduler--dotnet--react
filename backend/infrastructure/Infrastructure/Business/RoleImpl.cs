using System.Diagnostics;
using AppointmentScheduler.Domain;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class RoleImpl : BaseEntity, IRole
{
    internal const string DefaultRoleKey = "config.default.role";
    internal Role _role;

    internal RoleImpl(Role role) => _role = role ?? throw new ArgumentNullException(nameof(role));

    string IRole.Name { get => _role.Name; set => _role.Name = value; }
    string IRole.Description { get => _role.Description; set => _role.Description = value; }

    IEnumerable<Permission> IRole.Permissions => new PermissionEnumerable(_role.Permissions);

    bool IRole.IsNameValid => _role.Name.IsValidName();

    bool IRole.IsDescriptionValid => _role.Description.IsValidDescription();

    Task<bool> IRole.IsNameExisted()
        => !((IRole)this).IsNameValid ? Task.FromResult(false) : (
            from role in _dbContext.Set<Role>()
            where role.Id != _role.Id && role.Name == _role.Name
            select role
        ).AnyAsync();

    bool IRole.IsPermissionGranted(Permission permission)
    {
        uint permissionCode = (uint)permission;
        return ((((uint)_role.Permissions[permissionCode >>> 3]) >> (int)((~permissionCode) & 7)) & 1) != 0;
    }

    void IRole.SetPermissionGranted(Permission permission, bool granted)
    {
        var permissions = _role.Permissions;
        uint permissionCode = (uint)permission, x = permissionCode >>> 3, z = 1u << (int)((~permissionCode) & 7);
        permissions[x] = (byte)(granted ? permissions[x] | z : permissions[x] & ~z);
    }

    private Task<bool> IsValid()
        => !((IRole)this).IsDescriptionValid ? Task.FromResult(false)
            : ((IRole)this).IsNameExisted().InvertTaskResult();

    private Task<bool> CanDelete() => (
        from user in _dbContext.Set<User>()
        where user.RoleId == _role.Id
        select user
    ).AnyAsync().InvertTaskResult();

    protected override async Task<bool> Create()
    {
        var dataValid = await IsValid();
        if (dataValid) _dbContext.Add(_role);
        return dataValid;
    }

    protected override async Task<bool> Delete()
    {
        var canDelete = await CanDelete();
        if (canDelete) _dbContext.Remove(_role);
        return canDelete;
    }

    protected override async Task<bool> Update()
    {
        var dataValid = await IsValid();
        if (dataValid) _dbContext.Update(_role);
        return dataValid;
    }

    internal static async Task<IRole> GetDefault(IRepository repository)
    {
        Role role = default;
        var cp = await repository.GetService<IConfigurationPropertiesService>();
        var dc = await repository.GetService<DbContext>();

        if (!uint.TryParse(cp.GetProperty(DefaultRoleKey, null), out uint id))
        {
            var roles = dc.Set<Role>();
            if (await roles.CountAsync() == 1)
                role = await roles.FirstOrDefaultAsync();
        }
        else role = await dc.FindAsync<Role>(id);
        dc.GetService<ILogger<IRole>>().LogInformation(role?.Description ?? "<>");
        if (role == null)
        {
            if (!await dc.IdGenerated(role = new Role(), nameof(Role.Id)))
                throw new InvalidOperationException("RoleId overflowed.");
            var ir = await repository.Initialize((IRole)new RoleImpl(role));
            ir.Name = "System Administrators #" + role.Id;
            ir.Description = "This role is created by default.";
            Enum.GetValues<Permission>().GrantTo(ir);
            if (!await ir.Create() || !cp.SetProperty(DefaultRoleKey, role.Id.ToString()))
                throw new InvalidOperationException("The new default Role was not added.");
            return ir;
        }
        return await repository.Initialize((IRole)new RoleImpl(role));
    }
}