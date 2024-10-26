using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class PatientImpl : UserImpl, IPatient
{
    private readonly Patient _patient;
    public PatientImpl(User user, Patient patient) : base(user)
    {
        _patient = patient ?? throw new ArgumentNullException(nameof(patient));
    }

    string IPatient.Email { get => _patient.Email; set => _patient.Email = value; }
    string IPatient.Phone { get => _patient.Phone; set => _patient.Phone = value; }
    string IPatient.Image { get => _patient.Image; set => _patient.Image = value; }

    IEnumerable<IProfile> IPatient.LookupProfiles()
    {
        throw new NotImplementedException();
    }

    IProfile IPatient.ObtainProfile()
    {
        throw new NotImplementedException();
    }
}