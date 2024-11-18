using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IPatientService
	{
		IEnumerable<PatientViewModel> GetAllPatients();
		PatientViewModel GetPatientById(uint id);
		bool AddPatient(PatientViewModel patient);
		bool UpdatePatient(PatientViewModel patient);
		bool DeletePatient(uint id);
		int CountPatients();
	}
}
