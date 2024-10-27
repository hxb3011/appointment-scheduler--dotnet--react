namespace AppointmentScheduler.Domain.Entities;

public class PrescriptionDetail : BaseEntity
{
    public uint PrescriptionId { get; set; }
    public uint MedicineId { get; set; }
    public string Description { get; set; }
}