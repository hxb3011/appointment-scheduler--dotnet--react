using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class ProfileImpl : BaseEntity, IProfile
{
    private readonly Profile _profile;
    private readonly IPatient _patient;

    internal ProfileImpl(Profile profile, IPatient patient)
    {
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
    }

    IPatient IProfile.Patient => _patient;

    string IProfile.FullName { get => _profile.FullName; set => _profile.FullName = value; }
    DateOnly IProfile.DateOfBirth { get => _profile.DateOfBirth; set => _profile.DateOfBirth = value; }
    char IProfile.Gender { get => _profile.Gender; set => _profile.Gender = value; }

    IEnumerable<IAppointment> IProfile.Appointments { get => throw new NotImplementedException(); }

    IEnumerable<IExamination> IProfile.Examinations { get => throw new NotImplementedException(); }

    IAppointment IProfile.ObtainAppointment()
    {
        // TODO: Like IPatient.ObtainProfile();
        throw new NotImplementedException();
    }

    protected override Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Delete()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Initilize()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Update()
    {
        throw new NotImplementedException();
    }
}