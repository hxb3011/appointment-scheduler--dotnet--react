using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Infrastructure.Business;

namespace Infrastructure.Business
{
    public class AppointmentImpl : IAppointment
    {
        public int Number { get => throw new NotImplementedException(); }

        public DateTime AtTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IProfile Profile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IExamination Examination { get => new ExaminationImpl(); }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public IExamination ObtainExamination(IDoctor doctor)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}