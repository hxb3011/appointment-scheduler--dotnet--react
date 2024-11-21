using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Business;

internal class PrescriptionImpl : BaseEntity, IPrescription
{
    internal Prescription _prescription;
    private IExamination _examination;
    private IResourceManagerService _resourceManager;

    public PrescriptionImpl(Prescription prescription, IExamination examination)
    {
        _prescription = prescription;
        _examination = examination;
    }

    Stream IPrescription.Document(bool readOnly)
        => _resourceManager.Resource<PrescriptionImpl>(_prescription.Id.ToString(), readOnly);

    string IPrescription.Description { get => _prescription.Description; set => _prescription.Description = value; }

    public IEnumerable<IMedicine> Medicines { get => throw new NotImplementedException(); }

    protected override Task<bool> Create()
    {
        // TODO: Process Document
        _dbContext.Add(_prescription);
        return Task.FromResult(true);
    }

    protected override Task<bool> Delete()
    {
        // TODO: Process Document
        _dbContext.Remove(_prescription);
        return Task.FromResult(true);
    }

    protected override async Task<bool> Initilize()
    {
        _resourceManager = await _repository.GetService<IResourceManagerService>();
        return true;
    }

    protected override Task<bool> Update()
    {
        // TODO: Process Document
        _dbContext.Update(_prescription);
        return Task.FromResult(true);
    }
}