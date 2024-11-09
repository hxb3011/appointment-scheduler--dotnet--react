using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests
{
    public class CreatePrescriptionRequest
    {
        public uint ExaminationId { get; set; }
        public byte[] Document { get; set; }
        public string Description { get; set; }
    }
}
