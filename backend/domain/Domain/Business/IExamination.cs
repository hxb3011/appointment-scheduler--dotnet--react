namespace AppointmentScheduler.Domain.Business;

public interface IExamination : IBehavioralEntity
{
    IDoctor Doctor { get; }
    IAppointment Appointment { get; }
    string Diagnostic { get; set; }
    string Description { get; set; }
    uint State { get; set; }
    IPrescription Prescription { get; }
    IEnumerable<IDiagnosticService> DiagnosticServices { get; }
    Task<IPrescription> ObtainPrescription();
    Task<IDiagnosticService> ObtainDiagnostic(IDoctor doctor, IDiagnosticService diagnosticService);
}