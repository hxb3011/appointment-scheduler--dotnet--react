namespace AppointmentScheduler.Domain.Responses;

public class ExaminationResponse
{
    public uint Id { get; set; }
    public uint Appointment { get; set; }
    public string Diagnostic { get; set; }
    public string Description { get; set; }
    public uint State { get; set; }
}