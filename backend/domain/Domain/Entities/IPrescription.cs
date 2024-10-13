namespace AppointmentScheduler.Domain.Entities;

public interface IPrescription : IBehavioralEntity
{
    IDocument Document { get; set; }
    string Description { get; set; }
    IEnumerable<IMedicine> Medicines { get; }
}