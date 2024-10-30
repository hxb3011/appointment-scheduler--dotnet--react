namespace AppointmentScheduler.Domain.Business;

public interface IProfile : IBehavioralEntity
{
    IPatient Patient { get; }
    string FullName { get; set; }
    DateOnly DateOfBirth { get; set; }
    char Gender { get; set; }
    bool IsFullNameValid { get; }
    IEnumerable<IAppointment> Appointments { get; }
    IEnumerable<IExamination> Examinations { get; }
    Task<IAppointment> ObtainAppointment(DateTime atTime, IDoctor doctor);
}