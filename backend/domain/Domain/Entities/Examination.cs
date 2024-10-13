namespace AppointmentScheduler.Domain.Entities;

public class Examination
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentId { get; set; }
    public string Diagnostic { get; set; }
    public string Description { get; set; }
    public int State { get; set; }
}