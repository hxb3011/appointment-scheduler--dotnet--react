namespace AppointmentScheduler.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public string Certificate { get; set; }
    public string Image { get; set; }
}