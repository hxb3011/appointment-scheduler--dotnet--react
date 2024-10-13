namespace AppointmentScheduler.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] Permissions { get; set; }
}
