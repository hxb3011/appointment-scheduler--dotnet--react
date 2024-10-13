namespace AppointmentScheduler.Domain.Entities;

public interface IPatient : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    string Image { get; set; }
    IProfile ObtainProfile();
    IEnumerable<IProfile> LookupProfiles();
}