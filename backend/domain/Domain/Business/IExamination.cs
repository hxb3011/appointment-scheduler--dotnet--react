namespace AppointmentScheduler.Domain.Business;

public interface IExamination : IBehavioralEntity
{
    IDoctor Doctor { get; }
    IAppointment Appointment { get; }
    string Diagnostic { get; set; }
    string Description { get; set; }
    int State { get; set; }
    IPrescription Prescription { get; }
    IEnumerable<IDiagnosticService> DiagnosticServices { get; }
    IPrescription ObtainPrescription();
}