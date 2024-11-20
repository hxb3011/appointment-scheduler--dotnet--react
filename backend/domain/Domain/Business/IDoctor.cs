namespace AppointmentScheduler.Domain.Business;

public interface IDoctor : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    string Position { get; set; }
    string Certificate { get; set; }
    bool IsEmailValid { get; }
    bool IsPhoneValid { get; }
    IEnumerable<IAppointment> Appointments { get; }
    IEnumerable<IExamination> Examinations { get; }
    Task<IAppointment> ObtainAppointment(DateTime atTime, uint number);
    Task<IExamination> ObtainExamination(IAppointment appointment);
    Stream Image(bool readOnly = true);
}
