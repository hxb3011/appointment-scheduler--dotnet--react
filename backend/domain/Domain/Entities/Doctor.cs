namespace AppointmentScheduler.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public string Certificate { get; set; }
    // public string Image { get; set; }
}