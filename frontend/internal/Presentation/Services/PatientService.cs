using AppointmentScheduler.Presentation.Models;

namespace AppointmentScheduler.Presentation.Services;

public class PatientService
{
    private readonly List<PatientModel> _patients;

    public PatientService()
    {
        _patients = new List<PatientModel>();
    }

    public IEnumerable<PatientModel> GetAllPatients()
    {
        if (!_patients.Any())
        {
            _patients.Add(new PatientModel(1, "fdsaf", "342343212", "fjdsafkjdsj"));
            _patients.Add(new PatientModel(2, "fdsaf", "342343212", "fjdsafkjdsj"));
            _patients.Add(new PatientModel(3, "fdsaf", "342343212", "fjdsafkjdsj"));
            _patients.Add(new PatientModel(4, "fdsaf", "342343212", "fjdsafkjdsj"));
            _patients.Add(new PatientModel(5, "fdsaf", "342343212", "fjdsafkjdsj"));
            _patients.Add(new PatientModel(6, "fdsaf", "342343212", "fjdsafkjdsj"));
        }
        return _patients;
    }

    public PatientModel GetPatientById(uint id)
    {
        throw new NotImplementedException();
    }

    public bool AddPatient(PatientModel patient)
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

    public bool UpdatePatient(PatientModel patient)
    {
        throw new NotImplementedException();
    }
}