using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests
{
    public class CreatePrescriptionDetailRequest
    {
        public uint PrescriptionId { get; set; }
        public uint MedicineId { get; set; }
        public string Description { get; set; }
    }
}
