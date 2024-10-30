using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Infrastructure.Business;

internal class PrescriptionImpl : BaseEntity, IPrescription
{
    public IDocument Document { get => new DocumentImpl(); set => throw new NotImplementedException(); }
    public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<IMedicine> Medicines { get => new List<MedicineImpl>(); }

    protected override Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Delete()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Initilize()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Update()
    {
        throw new NotImplementedException();
    }
}