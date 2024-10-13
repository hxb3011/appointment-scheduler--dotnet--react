namespace AppointmentScheduler.Domain.Entities;

public class ExaminationService {
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string DiagnosticServiceId { get; set; }
    public string ExaminationId { get; set; }
    public byte[] Document { get; set; }
}