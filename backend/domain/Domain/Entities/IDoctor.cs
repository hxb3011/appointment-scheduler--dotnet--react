namespace AppointmentScheduler.Domain.Entities;

public interface IDoctor : IUser
{
    int Id { get; set; }
    string Email { get; set; }
    string Phone { get; set; }
    string Position { get; set; }
    string Certificate { get; set; }
    string Image { get; set; }
    IExamination ObtainExamination(IAppointment appointment);
    IEnumerable<IExamination> LookupExaminations();
}
