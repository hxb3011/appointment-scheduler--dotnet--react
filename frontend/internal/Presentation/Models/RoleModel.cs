using System.ComponentModel.DataAnnotations;
using AppointmentScheduler.Presentation.Models.Enums;

namespace AppointmentScheduler.Presentation.Models
{
    public class RoleModel
    {
        public uint Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public IDictionary<Permission, bool> Permissions { get; } = new Dictionary<Permission, bool>();
    }
}
