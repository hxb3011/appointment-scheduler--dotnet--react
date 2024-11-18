using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class PatientViewModel : BaseViewModel
	{
		public PatientViewModel(uint id, string email, string phone, string image)
		{
			Id = id;
			Email = email;
			Phone = phone;
			Image = image;
		}

		public string Email { get; set; }
		public string Phone {  get; set; }
		public string Image {  get; set; }
	}
}
