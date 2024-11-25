namespace AppointmentScheduler.Domain.Responses;

public class DoctorResponse
{
    public uint Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public string Certificate { get; set; }
}