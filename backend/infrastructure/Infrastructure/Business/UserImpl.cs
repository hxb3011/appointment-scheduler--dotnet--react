using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal abstract class UserImpl : BaseEntity, IUser
{
    internal readonly User _user;
    private IRole _role;
    internal UserImpl(User user, IRole role = null)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _role = role;
    }

    string IUser.UserName { get => _user.UserName; set => _user.UserName = value; }
    string IUser.Password { get => _user.Password; set => _user.Password = value; }
    string IUser.FullName { get => _user.FullName; set => _user.FullName = value; }
    IRole IUser.Role => _role;

    bool IUser.IsUserNameValid => _user.UserName.IsValidUserName();

    bool IUser.IsPasswordValid => _user.Password.IsValidPassword();

    bool IUser.IsFullNameValid => _user.FullName.IsValidName();

    async Task<bool> IUser.ChangeRole(IRole value)
    {
        if (value == null)
        {
            try
            {
                value = await RoleImpl.GetDefault(_repository);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
        _role = value;
        bool result = _repository.TryGetKeyOf(_role, out uint id);
        if (result) _user.RoleId = id;
        return result;
    }

    Task<bool> IUser.IsUserNameExisted()
        => !((IUser)this).IsUserNameValid ? Task.FromResult(false) : (
            from user in _dbContext.Set<User>()
            where user.Id != _user.Id && user.UserName.Equals(_user.UserName)
            select user
        ).AnyAsync();

    protected abstract Task<bool> CanDelete();

    protected virtual Task<bool> IsValid()
        => !((IUser)this).IsFullNameValid || !((IUser)this).IsPasswordValid
        ? Task.FromResult(false) : ((IUser)this).IsUserNameExisted().InvertTaskResult();

    protected override async Task<bool> Create()
    {
        bool dataValid = await IsValid();
        if (dataValid) _dbContext.Add(_user);
        return dataValid;
    }

    protected override async Task<bool> Delete()
    {
        bool canDelete = await CanDelete();
        if (canDelete) _dbContext.Remove(_user);
        return canDelete;
    }

    protected override async Task<bool> Update()
    {
        bool dataValid = await IsValid();
        if (dataValid) _dbContext.Update(_user);
        return dataValid;
    }
}