namespace AppointmentScheduler.Domain.Entities;

public interface IProfile : IBehavioralEntity
{
    IPatient Patient { get; set; }
    string FullName { get; set; }
    DateOnly DateOfBirth { get; set; }
    char Gender { get; set; }
    IAppointment ObtainAppointment();
    IEnumerable<IAppointment> LookupAppointments();
    IEnumerable<IExamination> LookupExaminations();
}