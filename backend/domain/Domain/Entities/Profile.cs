namespace AppointmentScheduler.Domain.Entities;

public class Profile
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public char Gender { get; set; }
}