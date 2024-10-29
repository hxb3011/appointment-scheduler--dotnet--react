using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class DoctorImpl : UserImpl, IDoctor
{
    private readonly Doctor _doctor;
    internal DoctorImpl(User user, Doctor doctor, IRole role = null) : base(user, role)
    {
        _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
    }

    string IDoctor.Email { get => _doctor.Email; set => _doctor.Email = value; }
    string IDoctor.Phone { get => _doctor.Phone; set => _doctor.Phone = value; }
    string IDoctor.Position { get => _doctor.Position; set => _doctor.Position = value; }
    string IDoctor.Certificate { get => _doctor.Certificate; set => _doctor.Certificate = value; }
    string IDoctor.Image { get => _doctor.Image; set => _doctor.Image = value; }
    bool IDoctor.IsEmailValid { get => _doctor.Email.IsValidEmail(emptyAllowed: true); }
    bool IDoctor.IsPhoneValid { get => _doctor.Phone.IsValidPhone(emptyAllowed: true); }

    IEnumerable<IExamination> IDoctor.Examinations { get => throw new NotImplementedException(); }

    Task<IExamination> IDoctor.ObtainExamination(IAppointment appointment)
    {
        throw new NotImplementedException();
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