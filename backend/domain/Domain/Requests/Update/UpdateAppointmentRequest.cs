using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.Update
{
    public class UpdateAppointmentRequest
    {
        public uint? ProfileId { get; set; }
        public uint? State { get; set; }
    }
}
