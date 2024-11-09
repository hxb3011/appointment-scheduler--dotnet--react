using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.ExaminationDetail
{
    public class CreateExamination
    {
        public uint AppointmentId { get; set; }
        public string Diagnostic { get; set; }
        public string Description { get; set; }
    }
}
