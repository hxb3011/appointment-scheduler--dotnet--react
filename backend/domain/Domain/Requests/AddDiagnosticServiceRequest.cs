using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Domain.Requests
{
    public class AddDiagnosticServiceRequest
    {
        [Required(ErrorMessage = "Doctor ID is required.")]
        public uint DoctorId { get; set; }

        [Required(ErrorMessage = "Diagnostic Service ID is required.")]
        public uint DiagnosticServiceId { get; set; }
    }
}
