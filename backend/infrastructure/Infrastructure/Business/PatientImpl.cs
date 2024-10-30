using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class PatientImpl : UserImpl, IPatient
{
    private readonly Patient _patient;
    private IEnumerable<IProfile> _profiles;
    internal PatientImpl(User user, Patient patient, IRole role) : base(user, role)
    {
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
    }

    string IPatient.Email { get => _patient.Email; set => _patient.Email = value; }
    string IPatient.Phone { get => _patient.Phone; set => _patient.Phone = value; }
    string IPatient.Image { get => _patient.Image; set => _patient.Image = value; }

    bool IPatient.IsEmailValid => _patient.Email.IsValidEmail(true);

    bool IPatient.IsPhoneValid => _patient.Phone.IsValidPhone(true);

    IEnumerable<IProfile> IPatient.Profiles
    {
        get => _profiles ??= (
            from p in _dbContext.Set<Profile>()
            where p.PatientId == _patient.Id
            select CreateProfile(p).WaitForResult(Timeout.Infinite, default)
        ).Cached();
    }

    async Task<IProfile> IPatient.ObtainProfile()
    {
        var profile = new Profile();
        if (!await _dbContext.IdGeneratedWrap(
            from p in _dbContext.Set<Profile>()
            where p.Id == profile.Id
            select p, profile, nameof(Profile.Id)
        )) return null;
        profile.PatientId = _patient.Id;
        return await CreateProfile(profile);
    }

    private Task<IProfile> CreateProfile(Profile profile)
    {
        IProfile impl = new ProfileImpl(profile, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _profiles = (IEnumerable<IProfile>)((ICloneable)_profiles).Clone();
    }

    protected override Task<bool> CanDelete() => (
        from e in _dbContext.Set<Profile>()
        where e.PatientId == _patient.Id
        select e
    ).AnyAsync().InvertTaskResult();

    protected override Task<bool> IsValid()
        => !((IPatient)this).IsEmailValid || !((IPatient)this).IsPhoneValid
            ? Task.FromResult(false) : base.IsValid();

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