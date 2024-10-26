using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class ExaminationImpl : BaseEntity, IExamination
    {
        public IDoctor Doctor { get => new DoctorImpl(); set => throw new NotImplementedException(); }
        public IAppointment Appointment { get => new AppointmentImpl(); set => throw new NotImplementedException(); }
        public string Diagnostic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IPrescription Prescription { get => new PrescriptionImpl(); }

        public IEnumerable<IDiagnosticService> DiagnosticServices { get => new List<DiagnosticServiceImpl>(); }

        public IPrescription ObtainPrescription()
        {
            throw new NotImplementedException();
        }

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
