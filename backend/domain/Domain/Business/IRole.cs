using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Domain.Business;

public interface IRole : IBehavioralEntity
{
    string Name { get; set; }
    string Description { get; set; }
    IEnumerable<Permission> Permissions { get; }
    bool IsNameValid { get; }
    bool IsDescriptionValid { get; }
    Task<bool> IsNameExisted();
    bool IsPermissionGranted(Permission permission);
    void SetPermissionGranted(Permission permission, bool granted = true);
}
