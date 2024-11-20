namespace AppointmentScheduler.Domain.Business;

public interface IDiagnosticService : IBehavioralEntity
{
    string Name { get; set; }
    double Price { get; set; }
    bool IsNameValid { get; }
    IDoctor Doctor { get; }
    Stream Document(bool readOnly = true);
}
