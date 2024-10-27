namespace AppointmentScheduler.Domain.Entities;

public class Profile : BaseEntity
{
    public uint PatientId { get; set; }
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public char Gender { get; set; }
}