using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    public class ProfileImpl : IProfile
    {
        public IPatient Patient { get => new PatientImpl(); set => throw new NotImplementedException(); }
        public string FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateOnly DateOfBirth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public char Gender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAppointment> LookupAppointments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IExamination> LookupExaminations()
        {
            throw new NotImplementedException();
        }

        public IAppointment ObtainAppointment()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
