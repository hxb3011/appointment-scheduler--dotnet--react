namespace AppointmentScheduler.Domain.Business;

public interface IDoctor : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    string Position { get; set; }
    string Certificate { get; set; }
    string Image { get; set; }
    IExamination ObtainExamination(IAppointment appointment);
    IEnumerable<IExamination> LookupExaminations();
}
