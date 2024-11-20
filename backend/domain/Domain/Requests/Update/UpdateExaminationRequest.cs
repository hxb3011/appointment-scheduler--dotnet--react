using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.Update
{
    public class UpdateExaminationRequest
    {
        public string Diagnostic { get; set; }
        public string Description { get; set; }
        public uint? State { get; set; }
    }
}
