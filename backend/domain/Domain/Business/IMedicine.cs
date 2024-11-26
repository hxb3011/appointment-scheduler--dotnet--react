namespace AppointmentScheduler.Domain.Business;

public interface IMedicine : IBehavioralEntity
{
    string Name { get; set; }
    // string Image { get; set; }
    string Unit { get; set; }
    string Description { get; set; }
}