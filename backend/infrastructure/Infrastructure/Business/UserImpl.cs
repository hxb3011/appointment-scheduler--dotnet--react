using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;

namespace AppointmentScheduler.Infrastructure.Business;

internal abstract class UserImpl : BaseEntity, IUser
{
    internal readonly User _user;
    private IRole _role;
    internal UserImpl(User user, IRole role)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _role = role ?? throw new ArgumentNullException(nameof(role));
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
    {
        if (!((IUser)this).IsUserNameValid) return Task.FromResult(false);
        var para = Expression.Parameter(typeof(User));
        return _dbContext.Set<User>().Where(Expression.Lambda<Func<User, bool>>(
            Expression.AndAlso(
                Expression.NotEqual(
                    Expression.Property(para, nameof(User.Id)),
                    Expression.Constant(_user.Id)
                ),
                Expression.Equal(
                    Expression.Property(para, nameof(User.UserName)),
                    Expression.Constant(_user.UserName)
                )
            ), para
        )).AnyAsync();
    }

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