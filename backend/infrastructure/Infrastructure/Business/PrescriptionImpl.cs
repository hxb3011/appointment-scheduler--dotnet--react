using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal class PrescriptionImpl : BaseEntity, IPrescription, IMedicine 
{
    internal Prescription _prescription;
    private IExamination _examination;
    private IEnumerable<IPrescriptionDetail> _prescriptionDetail;
    private IResourceManagerService _resourceManager;

    public PrescriptionImpl(Prescription prescription, IExamination examination)
    {
        _prescription = prescription;
        _examination = examination;
    }
    public string Description { get => _prescription.Description; set => _prescription.Description = value; }

    byte[] IPrescription.Document { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    IEnumerable<IPrescriptionDetail> IPrescription.PrescriptionDetails
    {
        get => _prescriptionDetail = (
            from pd in _dbContext.Set<PrescriptionDetail>()
            from md in _dbContext.Set<Medicine>()
            where pd.PrescriptionId == _prescription.Id && md.Id == pd.MedicineId
            select CreatePrescriptionDetail(pd, md).WaitForResult(Timeout.Infinite, default)
        ).Cached();
    }
    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Image { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Unit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private async Task<IPrescriptionDetail> CreatePrescriptionDetail(PrescriptionDetail pd, Medicine md = null)
    {
        var medicine = await _repository.GetEntityBy<uint, IMedicine>(md.Id);
        IPrescriptionDetail impl = new PrescriptionDetailImpl(pd, this, medicine);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return await _repository.Initialize(impl);
    }
    async Task<IPrescriptionDetail> IPrescription.ObtainPrescriptionDetail()
    {
        var prescriptionDetail = new PrescriptionDetail();
        if (!await _dbContext.IdGeneratedWrap(
            from pd in _dbContext.Set<PrescriptionDetail>()
            from md in _dbContext.Set<Medicine>()
            where pd.Id == prescriptionDetail.Id
            select pd, prescriptionDetail, nameof(PrescriptionDetail)
        )) return null;
        prescriptionDetail.PrescriptionId = _prescription.Id;
        return await CreatePrescriptionDetail(prescriptionDetail);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _prescriptionDetail = (IEnumerable<IPrescriptionDetail>)((ICloneable)((IPrescription)this).PrescriptionDetails).Clone();
    }
    private Task<bool> CanDelete() => (
        from pd in _dbContext.Set<PrescriptionDetail>()
        where pd.PrescriptionId == _prescription.Id
        select pd
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