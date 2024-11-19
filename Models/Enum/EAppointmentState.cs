using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enum
{
    public enum EAppointmentState
    {
        [Display(Name = "Không hoạt động")]
        DISABLE = 0,

        [Display(Name = "Hoạt động")]
        ENABLE = 1,

        [Display(Name = "Hết hạn")]
        EXPIRED = 2
    }
}
