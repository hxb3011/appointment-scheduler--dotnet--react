namespace AppointmentScheduler.Domain.Business;

public interface IAppointment : IBehavioralEntity
{
    int Number { get; }
    DateTime AtTime { get; }
    int State { get; set; }
    IProfile Profile { get; set; }
    IDoctor Doctor { get; }
    IExamination Examination { get; }
    Task<IExamination> ObtainExamination();
}
