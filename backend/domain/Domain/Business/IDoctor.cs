namespace AppointmentScheduler.Domain.Business;

public interface IDoctor : IUser
{
    string Email { get; set; }
    string Phone { get; set; }
    string Position { get; set; }
    string Certificate { get; set; }
    string Image { get; set; }
    bool IsEmailValid { get; }
    bool IsPhoneValid { get; }
    IEnumerable<IExamination> Examinations { get; }
    Task<IExamination> ObtainExamination(IAppointment appointment);
}
