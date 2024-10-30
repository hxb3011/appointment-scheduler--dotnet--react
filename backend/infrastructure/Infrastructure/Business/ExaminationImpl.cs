using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class ExaminationImpl : BaseEntity, IExamination
{
    internal Examination _examination;
    internal IAppointment _appointment;
    internal ExaminationImpl(Examination examination, IAppointment appointment)
    {
        _examination = examination ?? throw new ArgumentNullException(nameof(examination));
        _appointment = appointment ?? throw new ArgumentNullException(nameof(appointment));
    }

    IDoctor IExamination.Doctor { get => _appointment.Doctor; }
    IAppointment IExamination.Appointment { get => _appointment; }
    string IExamination.Diagnostic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    string IExamination.Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    int IExamination.State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    IPrescription IExamination.Prescription { get => new PrescriptionImpl(); }

    IEnumerable<IDiagnosticService> IExamination.DiagnosticServices { get => new List<DiagnosticServiceImpl>(); }

    IPrescription IExamination.ObtainPrescription()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Create()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Delete()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Initilize()
    {
        throw new NotImplementedException();
    }

    protected override Task<bool> Update()
    {
        throw new NotImplementedException();
    }
}
