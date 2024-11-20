using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Requests.Create
{
    public class CreateExaminationRequest
    {
        [Required(ErrorMessage = "Diagnostic thông tin là bắt buộc.")]
        public string Diagnostic { get; set; }

        [Required(ErrorMessage = "Description là bắt buộc.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "State là bắt buộc.")]
        public uint State { get; set; }

        [Required(ErrorMessage = "ID của cuộc hẹn là bắt buộc.")]
        public uint AppointmentId { get; set; }

        [Required(ErrorMessage = "ID của bác sĩ là bắt buộc.")]
        public uint DoctorId { get; set; }
    }
}
