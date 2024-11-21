namespace AppointmentScheduler.Presentation.Models;

public class PatientModel : Domain.Entities.Patient
{
	public PatientModel(uint id, string email, string phone, string image)
	{
		Id = id;
		Email = email;
		Phone = phone;
		Image = image;
	}
	public string Email { get; set; }
	public string Phone { get; set; }
	public string Image { get; set; }
}
