namespace AppointmentScheduler.Domain.IEntities;

public interface IExamination : IBehavioralEntity
{
    IDoctor Doctor { get; set; }
    IAppointment Appointment { get; set; }
    string Diagnostic { get; set; }
    string Description { get; set; }
    int State { get; set; }
    IPrescription Prescription { get; }
    IEnumerable<IDiagnosticService> DiagnosticServices { get; }
    IPrescription ObtainPrescription();
}