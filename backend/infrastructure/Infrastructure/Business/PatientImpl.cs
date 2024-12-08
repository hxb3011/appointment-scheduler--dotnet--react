using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class PatientImpl : UserImpl, IPatient
{
    internal readonly Patient _patient;
    private IResourceManagerService _resourceManager;
    private IEnumerable<IProfile> _profiles;
    private IEnumerable<IAppointment> _appointment;
    internal PatientImpl(User user, Patient patient, IRole role) : base(user, role)
    {
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
        ((IBehavioralEntity)this).Deleted += DeleteImage;
    }

    string IPatient.Email { get => _patient.Email; set => _patient.Email = value; }

    string IPatient.Phone { get => _patient.Phone; set => _patient.Phone = value; }

    bool IPatient.IsEmailValid => _patient.Email.IsValidEmail(true);

    bool IPatient.IsPhoneValid => _patient.Phone.IsValidPhone(true);

    IEnumerable<IProfile> IPatient.Profiles
    {
        get => _profiles ??= _dbContext.Set<Profile>().Where(p => p.PatientId == _patient.Id).ToList()
            .AsQueryable().Select(p => CreateProfile(p).WaitForResult(Timeout.Infinite, default)).Cached();
    }

    IEnumerable<IAppointment> IPatient.Appointments
    {
        get
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            return _appointment ??= _dbContext.Set<Profile>().SelectMany(p =>
                    _dbContext.Set<Appointment>().Where(a => p.Id == a.ProfileId && DateOnly.FromDateTime(a.AtTime) >= now))
                .OrderBy(a => a.AtTime).ToList().AsQueryable().Select(a =>
                    CreateAppointment(a).WaitForResult(Timeout.Infinite, default)).Cached();
        }
    }

    async Task<IProfile> IPatient.ObtainProfile()
    {
        var profile = new Profile();
        if (!await _dbContext.IdGenerated(profile, nameof(Profile.Id))) return null;
        profile.PatientId = _patient.Id;
        return await CreateProfile(profile);
    }

    Stream IPatient.Image(bool readOnly)
        => _resourceManager.Resource<PatientImpl>(_patient.Id.ToString(), readOnly);

    private void DeleteImage(object sender, EventArgs e)
        => _resourceManager.RemoveResource<PatientImpl>(_patient.Id.ToString());

    private Task<IProfile> CreateProfile(Profile profile)
    {
        IProfile impl = new ProfileImpl(profile, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
    }

    private async Task<IAppointment> CreateAppointment(Appointment appointment)
    {
        IAppointment impl = await _repository.GetEntityBy<Appointment, IAppointment>(appointment);
        if (impl != null)
        {
            impl.Created += InvalidateLoadedEntities;
            impl.Updated += InvalidateLoadedEntities;
            impl.Deleted += InvalidateLoadedEntities;
        }
        return impl;
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _profiles = null;
        _appointment = null;
    }

    protected override Task<bool> CanDelete() => (
        from e in _dbContext.Set<Profile>()
        where e.PatientId == _patient.Id
        select e
    ).AnyAsync().InvertTaskResult();

    protected override Task<bool> IsValid()
        => !((IPatient)this).IsEmailValid || !((IPatient)this).IsPhoneValid
            ? Task.FromResult(false) : base.IsValid();

    protected override async Task<bool> Initilize()
    {
        _resourceManager = await _repository.GetService<IResourceManagerService>();
        return true;
    }

    protected override async Task<bool> Create()
    {
        bool result = await base.Create();
        if (result) _dbContext.Add(_patient);
        return result;
    }

    protected override async Task<bool> Delete()
    {
        bool result = await base.Delete();
        if (result) _dbContext.Remove(_patient);
        return result;
    }

    protected override async Task<bool> Update()
    {
        bool result = await base.Update();
        if (result) _dbContext.Update(_patient);
        return result;
    }
}