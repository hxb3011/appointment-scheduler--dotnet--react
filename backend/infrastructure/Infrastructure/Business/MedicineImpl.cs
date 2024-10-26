using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class MedicineImpl : BaseEntity, IMedicine
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Image { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Unit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
    }
}
