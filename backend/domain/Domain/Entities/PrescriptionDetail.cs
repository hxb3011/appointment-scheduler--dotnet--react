namespace AppointmentScheduler.Domain.Entities;

public class PrescriptionDetail : BaseEntity
{
    public int PrescriptionId { get; set; }
    public int MedicineId { get; set; }
    public string Description { get; set; }
}