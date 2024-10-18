using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    public class UserImpl
    {
        public string UserName { get => "test@company.com"; set => throw new NotImplementedException(); }
        public string Password { get => "HeLlo|12"; set => throw new NotImplementedException(); }
        public string FullName { get => "Test"; set => throw new NotImplementedException(); }
        public IRole Role { get => new RoleImpl(); set => throw new NotImplementedException(); }

        public bool IsUserNameExisted => false;

        public bool IsUserNameValid => true;

        public bool IsPasswordValid => true;

        public bool IsFullNameValid => true;

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
