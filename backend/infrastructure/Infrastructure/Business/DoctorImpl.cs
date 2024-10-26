using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class DoctorImpl : UserImpl, IDoctor
{
    private readonly Doctor _doctor;
    internal DoctorImpl(User user, Doctor doctor) : base(user)
    {
        _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
    }

    string IDoctor.Email { get => _doctor.Email; set => _doctor.Email = value; }
    string IDoctor.Phone { get => _doctor.Phone; set => _doctor.Phone = value; }
    string IDoctor.Position { get => _doctor.Position; set => _doctor.Position = value; }
    string IDoctor.Certificate { get => _doctor.Certificate; set => _doctor.Certificate = value; }
    string IDoctor.Image { get => _doctor.Image; set => _doctor.Image = value; }

    IEnumerable<IExamination> IDoctor.LookupExaminations()
    {
        throw new NotImplementedException();
    }

    IExamination IDoctor.ObtainExamination(IAppointment appointment)
    {
        throw new NotImplementedException();
    }
}