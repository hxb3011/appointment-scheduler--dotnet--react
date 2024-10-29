using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class AppointmentImpl : BaseEntity, IAppointment, IRepositoryEntityInitializer
    {
        internal Appointment _appointment;
        internal IProfile _profile;
        internal IExamination _examination;
        internal IDoctor _doctor;

        public AppointmentImpl(Appointment appointment, IDoctor doctor, IProfile profile = null)
        {
            _appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
            _doctor = doctor;
            _profile = profile;
        }

        int IAppointment.Number { get => 0; }

        public DateTime AtTime { get => _appointment.AtTime; }

        public uint State { get => _appointment.State; set => _appointment.State = value; }

        public IDoctor Doctor { get => _doctor; }

        public IProfile Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                if(_repository.TryGetKeyOf(_profile, out uint id))
                {
                    _appointment.ProfileId = id;
                }
            }
        }
        public IExamination Examination { get => _examination; }

        public IExamination ObtainExamination()
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Create()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckExamination() => !await (
            from ex in _dbContext.Set<Examination>()
            where ex.AppointmentId == _appointment.Id
            select ex
            ).AnyAsync();


        protected override async Task<bool> Delete()
        {
            var canDelete = await CheckExamination();
            if (canDelete) _dbContext.Remove(_appointment);
            return canDelete;
        }

        protected override async Task<bool> Initilize()
        {
            //var result = await base.Initilize();
            _examination = await (
                from ex in _dbContext.Set<Examination>()
                where ex.Id == _appointment.Id
                select new ExaminationImpl(ex, this)
                ).FirstOrDefaultAsync();

            _doctor = await _repository.GetEntityBy<uint, IDoctor>(_appointment.DoctorId);

            return _doctor != null;
        }

        protected override Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}