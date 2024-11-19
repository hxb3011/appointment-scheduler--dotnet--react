using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Domain.Responses;

public class RoleResponse
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public static RoleResponse GetResponse(Role role)
        => new() { Id = role.Id, Name = role.Name, Description = role.Description };
}