namespace AppointmentScheduler.Domain.Entities;

public class User
{
    public const char GenderMale = 'M', GenderFemale = 'F';
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
}
