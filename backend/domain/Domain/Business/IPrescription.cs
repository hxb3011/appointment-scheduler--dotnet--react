namespace AppointmentScheduler.Domain.Business;

public interface IPrescription : IBehavioralEntity
{
    byte[] Document { get; set; }
    string Description { get; set; }
    IEnumerable<IPrescriptionDetail> PrescriptionDetails { get; }

    Task<IPrescriptionDetail> ObtainPrescriptionDetail();

}