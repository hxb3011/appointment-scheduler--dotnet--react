namespace AppointmentScheduler.Domain.Business;

public interface IPrescription : IBehavioralEntity
{
    IDocument Document { get; set; }
    string Description { get; set; }
    IEnumerable<IMedicine> Medicines { get; }
}