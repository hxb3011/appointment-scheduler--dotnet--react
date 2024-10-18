using AppointmentScheduler.Domain.IEntities;


namespace Infrastructure.ImplInterface
{
    public class AppointmentImpl : IAppointment 
    {
        public int Number { get; }
        public DateTime AtTime { get; set; }
        public int State { get; set; }
        public IProfile Profile { get; set; }
        public IExamination Examination { get; }

        public IExamination ObtainExamination(IDoctor doctor){
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}