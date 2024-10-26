namespace AppointmentScheduler.Domain.Business;

public interface IAppointment : IBehavioralEntity
{
    int Number { get; }
    TimeOnly AtTime { get; set; }
    int State { get; set; }
    IProfile Profile { get; set; }
    IExamination Examination { get; }
    IExamination ObtainExamination(IDoctor doctor);
}
