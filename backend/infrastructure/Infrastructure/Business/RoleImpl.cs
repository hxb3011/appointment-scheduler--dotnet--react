using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class RoleImpl : BaseEntity, IRole
{
    internal const string DefaultRoleKey = "config.default.role";
    private string _originalName;
    private Role _role;

    public RoleImpl(Role role) => _role = role ?? throw new ArgumentNullException(nameof(role));

    string IRole.Name { get => _role.Name; set => _role.Name = value; }
    string IRole.Description { get => _role.Description; set => _role.Description = value; }

    IEnumerable<Permission> IRole.Permissions => new PermissionEnumerator(_role.Permissions);

    bool IRole.IsNameValid => _role.Name.IsValidName();

    bool IRole.IsDescriptionValid => _role.Description.IsValidDescription();

    Task<bool> IRole.IsNameExisted()
        => !((IRole)this).IsNameValid ? Task.FromResult(false) : (
            from role in _dbContext.Set<Role>()
            where role.Id != _role.Id && (role.Name.Equals(_role.Name) || role.Name.Equals(_originalName))
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

    protected override Task<bool> Initilize()
        => Task.FromResult(true);

    protected override async Task<bool> Update()
    {
        var dataValid = await IsValid();
        if (dataValid) _dbContext.Update(_role);
        return dataValid;
    }
}