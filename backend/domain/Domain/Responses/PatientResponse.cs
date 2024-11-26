namespace AppointmentScheduler.Domain.Responses;

public class PatientResponse
{
    public uint Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public uint RoleId { get; set; }
}