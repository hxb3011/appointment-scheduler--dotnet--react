﻿using AppointmentScheduler.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class ProfileImpl : BaseEntity, IProfile
    {
        public IPatient Patient { get => new PatientImpl(); set => throw new NotImplementedException(); }
        public string FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateOnly DateOfBirth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public char Gender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
