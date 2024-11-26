namespace AppointmentScheduler.Domain.Requests;

public class UpdateExaminationRequest
{
    public string Diagnostic { get; set; }
    public string Description { get; set; }
    public uint State { get; set; }
}