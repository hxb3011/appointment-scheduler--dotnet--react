using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests
{
    public class UpdateMedicineRequest
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Unit { get; set; }
    }
}
