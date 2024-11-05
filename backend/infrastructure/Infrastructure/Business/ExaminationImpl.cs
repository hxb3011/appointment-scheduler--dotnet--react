using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class ExaminationImpl : BaseEntity, IExamination
{
    internal Examination _examination;
    internal IAppointment _appointment;
    internal Task<IPrescription> _prescriptionTask;
    internal IEnumerable<IDiagnosticService> _diagnosticServices;
    internal ExaminationImpl(Examination examination, IAppointment appointment)
    {
        _examination = examination ?? throw new ArgumentNullException(nameof(examination));
        _appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
    }

    IDoctor IExamination.Doctor { get => _appointment.Doctor; }
    IAppointment IExamination.Appointment { get => _appointment; }
    string IExamination.Diagnostic { get => _examination.Diagnostic; set => _examination.Diagnostic = value; }
    string IExamination.Description { get => _examination.Description; set => _examination.Description = value; }
    uint IExamination.State { get => _examination.State; set => _examination.State = value; }

    IPrescription IExamination.Prescription
        => (_prescriptionTask ??= (
            from ps in _dbContext.Set<Prescription>()
            where ps.ExaminationId == _examination.Id
            select CreatePrescription(ps).WaitForResult(Timeout.Infinite, default)
        ).FirstOrDefaultAsync()).WaitForResult();

    IEnumerable<IDiagnosticService> IExamination.DiagnosticServices
    {
        get => _diagnosticServices ??= (
            from es in _dbContext.Set<ExaminationService>()
            from ds in _dbContext.Set<DiagnosticService>()
            where es.ExaminationId == _examination.Id && es.DiagnosticServiceId == ds.Id
            select CreateDiagnosticServices(ds, es, null).WaitForResult(Timeout.Infinite, default)
        ).Cached();
    }

    async Task<IPrescription> IExamination.ObtainPrescription()
    {
        var prescription = new Prescription();
        if (!await _dbContext.IdGeneratedWrap(
            from ap in _dbContext.Set<Prescription>()
            where ap.Id == prescription.Id
            select ap, prescription, nameof(Prescription.Id)
        )) return null;
        prescription.ExaminationId = _examination.Id;
        return await CreatePrescription(prescription);
    }

    async Task<IDiagnosticService> IExamination.ObtainDiagnostic(IDoctor doctor, IDiagnosticService diagnosticService)
    {
        if (diagnosticService is not DiagnosticServiceImpl impl) return null;
        if (!_repository.TryGetKeyOf(doctor, out uint id)) return null;
        var diagsv = impl._diagsv;
        var exdiag = new ExaminationService();
        if (!await _dbContext.IdGeneratedWrap(
            from es in _dbContext.Set<ExaminationService>()
            where es.Id == exdiag.Id
            select es, exdiag, nameof(ExaminationService.Id)
        )) return null;
        exdiag.DoctorId = id;
        return await CreateDiagnosticServices(diagsv, exdiag, doctor);
    }

    private Task<IPrescription> CreatePrescription(Prescription ps)
    {
        IPrescription impl = new PrescriptionImpl(ps, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
        throw new NotImplementedException();
    }

    private async Task<IDiagnosticService> CreateDiagnosticServices(DiagnosticService diagsv, ExaminationService exdiag, IDoctor doctor = null)
    {
        IDiagnosticService impl = new DiagnosticServiceImpl(diagsv, exdiag, doctor ?? await _repository.GetEntityBy<uint, IDoctor>(exdiag.DoctorId), this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return await _repository.Initialize(impl);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _diagnosticServices = (IEnumerable<IDiagnosticService>)((ICloneable)((IExamination)this).DiagnosticServices).Clone();
        _prescriptionTask = null;
    }

    protected override Task<bool> Create()
    {
        _dbContext.Add(_examination);
        return Task.FromResult(true);
    }

    protected override async Task<bool> Delete()
    {
        if (await (
            from ps in _dbContext.Set<Prescription>()
            where ps.ExaminationId == _examination.Id
            select ps
        ).AnyAsync() || await (
            from es in _dbContext.Set<ExaminationService>()
            where es.ExaminationId == _examination.Id
            select es
        ).AnyAsync()) return false;

        _dbContext.Remove(_examination);
        return true;
    }

    protected override Task<bool> Update()
    {
        _dbContext.Update(_examination);
        return Task.FromResult(true);
    }
}
