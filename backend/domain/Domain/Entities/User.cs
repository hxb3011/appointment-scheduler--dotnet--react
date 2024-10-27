namespace AppointmentScheduler.Domain.Entities;

public class User : BaseEntity
{
    public const char GenderMale = 'M', GenderFemale = 'F';
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public uint RoleId { get; set; }
}
