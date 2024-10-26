namespace AppointmentScheduler.Domain.Entities;

public class Examination : BaseEntity
{
    public int DoctorId { get; set; }
    public int AppointmentId { get; set; }
    public string Diagnostic { get; set; }
    public string Description { get; set; }
    public int State { get; set; }
}