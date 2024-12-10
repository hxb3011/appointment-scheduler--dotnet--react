using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Business;

internal class PrescriptionImpl : BaseEntity, IPrescription
{
    internal Prescription _prescription;
    private IResourceManagerService _resourceManager;

    public PrescriptionImpl(Prescription prescription)
    {
        _prescription = prescription ?? throw new ArgumentNullException(nameof(prescription));
        ((IBehavioralEntity)this).Deleted += DeleteDocument;
    }

    Stream IPrescription.Document(bool readOnly)
        => _resourceManager.Resource<PrescriptionImpl>(_prescription.Id.ToString(), readOnly);

    private void DeleteDocument(object sender, EventArgs e)
        => _resourceManager.RemoveResource<PrescriptionImpl>(_prescription.Id.ToString());

    string IPrescription.Description { get => _prescription.Description; set => _prescription.Description = value; }

    public IEnumerable<IMedicine> Medicines { get => throw new NotImplementedException(); }

    protected override Task<bool> Create()
    {
        _dbContext.Add(_prescription);
        return Task.FromResult(true);
    }

    protected override Task<bool> Delete()
    {
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