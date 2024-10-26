using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class AppointmentImpl : BaseEntity, IAppointment
    {
        int IAppointment.Number => throw new NotImplementedException();

        DateTime IAppointment.AtTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IAppointment.State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IProfile IAppointment.Profile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IExamination IAppointment.Examination => throw new NotImplementedException();

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

        IExamination IAppointment.ObtainExamination(IDoctor doctor)
        {
            throw new NotImplementedException();
        }
    }
}