namespace AppointmentScheduler.Domain.Business;

public interface IPrescription : IBehavioralEntity
{
    string Description { get; set; }
    IEnumerable<IMedicine> Medicines { get; }
    Stream Document(bool readOnly = true);
}