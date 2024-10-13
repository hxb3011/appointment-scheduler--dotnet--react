namespace AppointmentScheduler.Domain.Entities;

public class PrescriptionDetail {
    public int Id { get; set; }
    public int PrescriptionId { get; set; }
    public int MedicineId { get; set; }
    public string Description { get; set; }
}