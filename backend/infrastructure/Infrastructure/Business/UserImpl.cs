using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal abstract class UserImpl : BaseEntity, IUser
{
    private readonly string _originalUserName;
    internal readonly User _user;
    internal readonly Role _role;
    internal UserImpl(User user) => _originalUserName = (_user = user ?? throw new ArgumentNullException(nameof(user))).UserName;
    string IUser.UserName { get => _user.UserName; set => _user.UserName = value; }
    string IUser.Password { get => _user.Password; set => _user.Password = value; }
    string IUser.FullName { get => _user.FullName; set => _user.FullName = value; }
    IRole IUser.Role { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    bool IUser.IsUserNameExisted => throw new NotImplementedException();

    bool IUser.IsUserNameValid => _user.UserName.IsValidUserName();

    bool IUser.IsPasswordValid => _user.Password.IsValidPassword();

    bool IUser.IsFullNameValid => _user.FullName.IsValidName();

    protected virtual Task<bool> CanDelete() {
        throw new NotImplementedException();
    }

    protected virtual Task<bool> IsValid() {
        throw new NotImplementedException();
    }

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

    protected override Task<bool> Initilize()
    {
        throw new NotImplementedException();
    }

    protected override async Task<bool> Update()
    {
        bool dataValid = await IsValid();
        if (dataValid) _dbContext.Update(_user);
        return dataValid;
    }
}