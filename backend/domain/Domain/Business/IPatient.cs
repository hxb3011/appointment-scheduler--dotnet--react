namespace AppointmentScheduler.Domain.Business;

public interface IPatient : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    string Image { get; set; }
    bool IsEmailValid { get; }
    bool IsPhoneValid { get; }
    IEnumerable<IProfile> Profiles { get; }
    Task<IProfile> ObtainProfile();
}