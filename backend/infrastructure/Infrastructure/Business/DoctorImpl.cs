using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class DoctorImpl : UserImpl, IDoctor
{
    internal readonly Doctor _doctor;
    private IResourceManagerService _resourceManager;
    private IEnumerable<IAppointment> _appointments;
    private IEnumerable<IExamination> _examinations;
    private IEnumerable<IDiagnosticService> _diagnosticServices;

    internal DoctorImpl(User user, Doctor doctor, IRole role) : base(user, role)
    {
        _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
        ((IBehavioralEntity)this).Deleted += DeleteImage;
    }

    string IDoctor.Email { get => _doctor.Email; set => _doctor.Email = value; }
    string IDoctor.Phone { get => _doctor.Phone; set => _doctor.Phone = value; }
    string IDoctor.Position { get => _doctor.Position; set => _doctor.Position = value; }
    string IDoctor.Certificate { get => _doctor.Certificate; set => _doctor.Certificate = value; }
    bool IDoctor.IsEmailValid { get => _doctor.Email.IsValidEmail(emptyAllowed: true); }
    bool IDoctor.IsPhoneValid { get => _doctor.Phone.IsValidPhone(emptyAllowed: true); }
    IEnumerable<IAppointment> IDoctor.Appointments
    {
        get => _appointments ??= (
                from ap in _dbContext.Set<Appointment>()
                where ap.DoctorId == _doctor.Id
                orderby ap.AtTime ascending
                select ap).ToList().AsQueryable()
            .Select(ap => CreateAppointment(ap).WaitForResult(Timeout.Infinite, default));
    }

    IEnumerable<IExamination> IDoctor.Examinations
    {
        get => _examinations ??= (
            from ap in ((IDoctor)this).Appointments
            where ap.Examination != null
            select ap.Examination
        );
    }

    IEnumerable<IDiagnosticService> IDoctor.DiagnosticServices
    {
        get
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            return _diagnosticServices ??= (
            from es in _dbContext.Set<ExaminationService>()
            from ds in _dbContext.Set<DiagnosticService>()
            from ex in _dbContext.Set<Examination>()
            from ap in _dbContext.Set<Appointment>()
            where es.ExaminationId == ex.Id && ex.AppointmentId == ap.Id
                && es.DoctorId == _doctor.Id && es.DiagnosticServiceId == ds.Id
                && DateOnly.FromDateTime(ap.AtTime) == now
            orderby ap.AtTime ascending
            select CreateDiagnosticServices(ds, es).WaitForResult(Timeout.Infinite, default)
        ).Cached();
        }
    }

    async Task<IAppointment> IDoctor.ObtainAppointment(DateTime atTime, uint number)
    {
        var appointment = new Appointment();
        if (!await _dbContext.IdGenerated(appointment, nameof(Appointment.Id))) return null;
        appointment.AtTime = atTime;
        appointment.Number = number;
        appointment.DoctorId = _doctor.Id;
        return await CreateAppointment(appointment);
    }

    Task<IExamination> IDoctor.ObtainExamination(IAppointment appointment)
        => appointment.ObtainExamination();

    Stream IDoctor.Image(bool readOnly)
        => _resourceManager.Resource<DoctorImpl>(_doctor.Id.ToString(), readOnly);

    private void DeleteImage(object sender, EventArgs e)
        => _resourceManager.RemoveResource<DoctorImpl>(_doctor.Id.ToString());

    private Task<IAppointment> CreateAppointment(Appointment appointment)
    {
        IAppointment impl = new AppointmentImpl(appointment, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
    }

    private async Task<IDiagnosticService> CreateDiagnosticServices(DiagnosticService diagsv, ExaminationService exdiag)
    {
        IDiagnosticService impl = new DiagnosticServiceImpl(diagsv, exdiag, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return await _repository.Initialize(impl);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _examinations = null;
        _appointments = null;
        _diagnosticServices = null;
    }

    protected override async Task<bool> Initilize()
    {
        _resourceManager = await _repository.GetService<IResourceManagerService>();
        return true;
    }

    protected override async Task<bool> CanDelete()
        => !await (
            from e in _dbContext.Set<Appointment>()
            where e.DoctorId == _doctor.Id
            select e
        ).AnyAsync() && !await (
            from e in _dbContext.Set<ExaminationService>()
            where e.DoctorId == _doctor.Id
            select e
        ).AnyAsync();

    protected override Task<bool> IsValid()
        => !((IDoctor)this).IsEmailValid || !((IDoctor)this).IsPhoneValid
            ? Task.FromResult(false) : base.IsValid();

    protected override async Task<bool> Create()
    {
        bool result = await base.Create();
        if (result) _dbContext.Add(_doctor);
        return result;
    }

    protected override async Task<bool> Delete()
    {
        bool result = await base.Delete();
        if (result) _dbContext.Remove(_doctor);
        return result;
    }

    protected override async Task<bool> Update()
    {
        bool result = await base.Update();
        if (result) _dbContext.Update(_doctor);
        return result;
    }
}