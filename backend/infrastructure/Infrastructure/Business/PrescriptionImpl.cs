using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public IDocument Document { get => new DocumentImpl(); set => throw new NotImplementedException(); }
    public string Description { get => _prescription.Description; set => _prescription.Description = value; }

    public IEnumerable<IMedicine> Medicines { get => throw new NotImplementedException(); }

    private Task<bool> CanDelete() => (
        from ap in _dbContext.Set<Examination>()
        where ap.Id == _prescription.Id
        select ap
    ).AnyAsync();
    protected override Task<bool> Create()
    {
        // TODO: Process Document
        _dbContext.Add(_prescription);
        return Task.FromResult(true);
    }

    protected override async Task<bool> Delete()
    {
        // TODO: Process Document
        var canDelete = await CanDelete();
        if (canDelete) _dbContext.Remove(_prescription);
        return canDelete;
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