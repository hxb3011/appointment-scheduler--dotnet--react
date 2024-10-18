using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Domain.IEntities;

public interface IRole : IBehavioralEntity
{
    string Name { get; set; }
    string Description { get; set; }
    IEnumerable<Permission> Permissions{ get; }
    bool IsNameExisted { get; }
    bool IsNameValid { get; }
    bool IsDescriptionValid { get; }
    bool IsPermissionGranted(Permission permission);
    bool SetPermissionGranted(Permission permission, bool granted = true);
}
