namespace AppointmentScheduler.Domain.Business;

public interface IUser : IBehavioralEntity
{
    string UserName { get; set; }
    string Password { get; set; }
    string FullName { get; set; }
    IRole Role { get; set; }
    bool IsUserNameValid { get; }
    bool IsPasswordValid { get; }
    bool IsFullNameValid { get; }
    Task<bool> IsUserNameExisted();
}
