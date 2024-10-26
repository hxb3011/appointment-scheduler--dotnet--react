namespace AppointmentScheduler.Domain.Entities;

public class Medicine : BaseEntity
{
    public string Name { get; set; }
    public string Image { get; set; }
    public string Unit { get; set; }
}