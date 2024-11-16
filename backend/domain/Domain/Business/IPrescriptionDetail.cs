using AppointmentScheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Business
{
    public interface IPrescriptionDetail: IBehavioralEntity
    {
        IPrescription Prescription { get; }
        
        IMedicine Medicine { get; set; }
        string Description { get; set; }
    }
}
