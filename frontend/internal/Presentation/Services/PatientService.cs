using Models;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
	public class PatientService : IPatientService
	{
		private readonly List<PatientViewModel> _patients;

		public PatientService()
		{
			_patients = new List<PatientViewModel>();
		}

		public IEnumerable<PatientViewModel> GetAllPatients()
		{
			if (!_patients.Any())
			{
				_patients.Add(new PatientViewModel(1, "fdsaf", "342343212", "fjdsafkjdsj"));
				_patients.Add(new PatientViewModel(2, "fdsaf", "342343212", "fjdsafkjdsj"));
				_patients.Add(new PatientViewModel(3, "fdsaf", "342343212", "fjdsafkjdsj"));
				_patients.Add(new PatientViewModel(4, "fdsaf", "342343212", "fjdsafkjdsj"));
				_patients.Add(new PatientViewModel(5, "fdsaf", "342343212", "fjdsafkjdsj"));
				_patients.Add(new PatientViewModel(6, "fdsaf", "342343212", "fjdsafkjdsj"));
			}
			return _patients;
		}

		public PatientViewModel GetPatientById(uint id)
		{
			throw new NotImplementedException();
		}

		public bool AddPatient(PatientViewModel patient)
		{
			throw new NotImplementedException();
		}

		public int CountPatients()
		{
			throw new NotImplementedException();
		}

		public bool DeletePatient(uint id)
		{
			throw new NotImplementedException();
		}

		public bool UpdatePatient(PatientViewModel patient)
		{
			throw new NotImplementedException();
		}
	}
}
