using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class AppointmentImpl : BaseEntity, IAppointment
{
    internal Appointment _appointment;
    private IProfile _profile;
    private Task<IExamination> _examinationTask;
    private IDoctor _doctor;

    internal AppointmentImpl(Appointment appointment, IDoctor doctor, IProfile profile = null)
    {
        _appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
        _doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
        _profile = profile;
    }

    uint IAppointment.Number { get => _appointment.Number; }
    DateTime IAppointment.AtTime { get => _appointment.AtTime; }
    uint IAppointment.State { get => _appointment.State; set => _appointment.State = value; }
    IProfile IAppointment.Profile
    {
        get
        {
            var profileId = _appointment.ProfileId;
            if (profileId.HasValue)
            {
                _profile = _repository.GetEntityBy<uint, IProfile>(profileId.Value).WaitForResult();
                return _profile;
            }
            return null;
        }

        set
        {
            if (_repository.TryGetKeyOf(value, out uint id))
            {
                _profile = value;
                _appointment.ProfileId = id;
            }
        }
    }

    IDoctor IAppointment.Doctor => _doctor;
    IExamination IAppointment.Examination => (_examinationTask
        ??= GetExaminations().Select(CreateExamination).FirstOrDefault())?.WaitForResult();

    async Task<IExamination> IAppointment.ObtainExamination()
    {
        var examination = new Examination();
        if (!await _dbContext.IdGenerated(examination, nameof(Examination.Id))) return null;
        examination.AppointmentId = _appointment.Id;
        return await CreateExamination(examination);
    }

    private Task<IExamination> CreateExamination(Examination examination)
    {
        IExamination impl = new ExaminationImpl(examination, this);
        impl.Created += InvalidateLoadedEntities;
        impl.Updated += InvalidateLoadedEntities;
        impl.Deleted += InvalidateLoadedEntities;
        return _repository.Initialize(impl);
    }

    private void InvalidateLoadedEntities(object sender, EventArgs e)
    {
        _examinationTask = null;
    }

    protected override Task<bool> Create()
    {
        _dbContext.Add(_appointment);
        return Task.FromResult(true);
    }

    public Task<bool> CheckExamination() => GetExaminations().AnyAsync().InvertTaskResult();

    private IQueryable<Examination> GetExaminations()
    {
        var param = Expression.Parameter(typeof(Examination));
        return _dbContext.Set<Examination>()
            .Where(Expression.Lambda<Func<Examination, bool>>(
                Expression.Equal(
                    Expression.Property(param, nameof(Examination.AppointmentId)),
                    Expression.Constant(_appointment.Id)
                ), param
            ));
    }

    protected override async Task<bool> Delete()
    {
        var canDelete = await CheckExamination();
        if (canDelete) _dbContext.Remove(_appointment);
        return canDelete;
    }

    protected override Task<bool> Update()
    {
        _dbContext.Update(_appointment);
        return Task.FromResult(true);
    }
}