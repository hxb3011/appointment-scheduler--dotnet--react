namespace AppointmentScheduler.Domain.IEntities;

public interface IAppointment : IBehavioralEntity
{
    int Number { get; }
    DateTime AtTime { get; set; }
    int State { get; set; }
    IProfile Profile { get; set; }
    IExamination Examination { get; }
    IExamination ObtainExamination(IDoctor doctor);
}
