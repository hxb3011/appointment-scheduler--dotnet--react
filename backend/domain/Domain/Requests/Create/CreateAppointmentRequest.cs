using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.Create
{
    public class CreateAppointmentRequest
    {
        public DateTime AtTime { get; set; }
        public uint Number { get; set; }
        public uint State { get; set; }
        public uint? ProfileId { get; set; }
        public uint DoctorId { get; set; }
    }
}
