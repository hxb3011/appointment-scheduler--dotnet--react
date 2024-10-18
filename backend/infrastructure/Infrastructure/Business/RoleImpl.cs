using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    public class RoleImpl : IRole
    {
        public string Name { get => "Role"; set => throw new NotImplementedException(); }
        public string Description { get => "Description"; set => throw new NotImplementedException(); }

        public IEnumerable<Permission> Permissions => [Permission.Perm2, Permission.Perm3];

        public bool IsNameExisted => false;

        public bool IsNameValid => true;

        public bool IsDescriptionValid => true;

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool IsPermissionGranted(Permission permission)
        {
            return permission == Permission.Perm2 || permission == Permission.Perm3;
        }

        public bool SetPermissionGranted(Permission permission, bool granted = true)
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
