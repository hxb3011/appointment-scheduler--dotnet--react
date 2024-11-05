using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class ProfileImpl : BaseEntity, IProfile
{
    internal readonly Profile _profile;
    private readonly IPatient _patient;
    private IEnumerable<IAppointment> _appointments;
    private IEnumerable<IExamination> _examinations;

    internal ProfileImpl(Profile profile, IPatient patient)
    {
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
    }

    IPatient IProfile.Patient => _patient;
    string IProfile.FullName { get => _profile.FullName; set => _profile.FullName = value; }
    DateOnly IProfile.DateOfBirth { get => _profile.DateOfBirth; set => _profile.DateOfBirth = value; }
    char IProfile.Gender { get => _profile.Gender; set => _profile.Gender = value; }
    bool IProfile.IsFullNameValid { get => _profile.FullName.IsValidName(); }
    IEnumerable<IAppointment> IProfile.Appointments
    {
        get => _appointments ??= (
            from ap in _dbContext.Set<Appointment>()
            where ap.ProfileId == _profile.Id
            select CreateAppointment(ap).WaitForResult(Timeout.Infinite, default)
        ).Cached();
    }

    IEnumerable<IExamination> IProfile.Examinations
    {
        get => _examinations ??= (
            from ap in ((IProfile)this).Appointments
            where ap.Examination != null
            select ap.Examination
        );
    }

    async Task<IAppointment> IProfile.ObtainAppointment(DateTime atTime, IDoctor doctor)
    {
        var impl = await doctor.ObtainAppointment(atTime);
        impl.Profile = this;
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return impl;
    }

    private async Task<IAppointment> CreateAppointment(Appointment appointment)
    {
        IAppointment impl = new AppointmentImpl(appointment,
            await _repository.GetEntityBy<uint, IDoctor>(appointment.DoctorId));
        impl = await _repository.Initialize(impl);
        impl.Profile = this;
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return impl;
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _appointments = (IEnumerable<IAppointment>)((ICloneable)((IProfile)this).Appointments).Clone();
    }

    private Task<bool> CanDelete() => (
        from ap in _dbContext.Set<Appointment>()
        where ap.ProfileId == _profile.Id
        select ap
    ).AnyAsync().InvertTaskResult();

    private Task<bool> IsValid() => Task.FromResult(((IProfile)this).IsFullNameValid);

    protected override async Task<bool> Create()
    {
        var dataValid = await IsValid();
        if (dataValid) _dbContext.Add(_profile);
        return dataValid;
    }

    protected override async Task<bool> Delete()
    {
        var canDelete = await CanDelete();
        if (canDelete) _dbContext.Remove(_profile);
        return canDelete;
    }

    protected override async Task<bool> Update()
    {
        var dataValid = await IsValid();
        if (dataValid) _dbContext.Update(_profile);
        return dataValid;
    }
}