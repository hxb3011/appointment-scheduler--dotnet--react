using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.Appointment
{
    public class CreateAppointmentRequest
    {
        public DateTime AtTime { get; set; }
        public uint? ProfileId { get; set; }
        public uint DoctorId { get; set; }
    }
}
