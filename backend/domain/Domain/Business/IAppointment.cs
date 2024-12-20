namespace AppointmentScheduler.Domain.Business;

public interface IAppointment : IBehavioralEntity
{
    uint Number { get; }
    DateTime AtTime { get; }
    uint State { get; set; }
    IDoctor Doctor { get; }
    IProfile Profile { get; set; }
    IExamination Examination { get; }
    Task<IExamination> ObtainExamination();
}
