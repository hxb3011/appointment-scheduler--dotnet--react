namespace AppointmentScheduler.Domain.Requests;

public class RegisterRequest
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}