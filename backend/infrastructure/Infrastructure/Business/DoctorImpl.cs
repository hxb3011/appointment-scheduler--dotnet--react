using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    public class DoctorImpl : IDoctor
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Phone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Certificate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Image { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRole Role { get => new RoleImpl(); set => throw new NotImplementedException(); }

        public bool IsUserNameExisted { get => throw new NotImplementedException(); }

        public bool IsUserNameValid { get => throw new NotImplementedException(); }

        public bool IsPasswordValid { get => throw new NotImplementedException(); } 

        public bool IsFullNameValid { get => throw new NotImplementedException(); }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IExamination> LookupExaminations()
        {
            throw new NotImplementedException();
        }

        public IExamination ObtainExamination(IAppointment appointment)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
