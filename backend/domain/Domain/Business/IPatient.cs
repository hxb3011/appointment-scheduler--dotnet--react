namespace AppointmentScheduler.Domain.Business;

public interface IPatient : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    bool IsEmailValid { get; }
    bool IsPhoneValid { get; }
    IEnumerable<IProfile> Profiles { get; }
    Task<IProfile> ObtainProfile();
    Stream Image(bool readOnly = true);
}