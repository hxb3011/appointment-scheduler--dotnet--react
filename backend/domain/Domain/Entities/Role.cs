namespace AppointmentScheduler.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] Permissions { get; set; } = new byte[60];
}
