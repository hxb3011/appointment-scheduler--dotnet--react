using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Infrastructure.Business;

namespace Infrastructure.Business
{
    public class DiagnosticServiceImpl : IDiagnosticService
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDoctor Doctor { get => new DoctorImpl(); set => throw new NotImplementedException(); }
        public IDocument Document { get => new DocumentImpl(); set => throw new NotImplementedException(); }

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