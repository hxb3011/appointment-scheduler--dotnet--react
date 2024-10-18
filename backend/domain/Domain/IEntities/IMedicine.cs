namespace AppointmentScheduler.Domain.IEntities;

public interface IMedicine : IBehavioralEntity
{
    string Name { get; set; }
    string Image { get; set; }
    string Unit { get; set; }
    string Description { get; set; }
}