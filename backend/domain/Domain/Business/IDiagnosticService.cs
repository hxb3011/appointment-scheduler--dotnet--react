namespace AppointmentScheduler.Domain.Business;

public interface IDiagnosticService : IBehavioralEntity
{
    string Name { get; set; }
    double Price { get; set; }
    IDoctor Doctor { get; set; }
    IDocument Document { get; set; }
}
