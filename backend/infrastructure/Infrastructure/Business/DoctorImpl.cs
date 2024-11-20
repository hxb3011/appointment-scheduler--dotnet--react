using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class DoctorImpl : UserImpl, IDoctor
{
    private IResourceManagerService _resourceManager;
    private readonly Doctor _doctor;
    private IEnumerable<IAppointment> _appointments;
    private IEnumerable<IExamination> _examinations;
    internal DoctorImpl(User user, Doctor doctor, IRole role) : base(user, role)
    {
        _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
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
            select CreateAppointment(ap).WaitForResult(Timeout.Infinite, default)
        ).Cached();
    }

    IEnumerable<IExamination> IDoctor.Examinations
    {
        get => _examinations ??= (
            from ap in ((IDoctor)this).Appointments
            where ap.Examination != null
            select ap.Examination
        );
    }

    async Task<IAppointment> IDoctor.ObtainAppointment(DateTime atTime, uint number)
    {
        var appointment = new Appointment();
        if (!await _dbContext.IdGeneratedWrap(
            from ap in _dbContext.Set<Appointment>()
            where ap.Id == appointment.Id
            select ap, appointment, nameof(Appointment.Id)
        )) return null;
        appointment.AtTime = atTime;
        appointment.Number = number;
        appointment.DoctorId = _doctor.Id;
        return await CreateAppointment(appointment);
    }

    Task<IExamination> IDoctor.ObtainExamination(IAppointment appointment)
        => appointment.ObtainExamination();

    Stream IDoctor.Image(bool readOnly)
        => _resourceManager.Resource<DoctorImpl>(_doctor.Id, readOnly);

    private Task<IAppointment> CreateAppointment(Appointment appointment)
    {
        IAppointment impl = new AppointmentImpl(appointment, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _appointments = (IEnumerable<IAppointment>)((ICloneable)((IDoctor)this).Appointments).Clone();
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