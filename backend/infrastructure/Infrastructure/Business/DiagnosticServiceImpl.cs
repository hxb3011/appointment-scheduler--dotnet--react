using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class DiagnosticServiceImpl : BaseEntity, IDiagnosticService
    {
        internal DiagnosticService _diagsv;
        private ExaminationService _exdiag;
        private IDoctor _doctor;
        internal DiagnosticServiceImpl(DiagnosticService diagnosticService, ExaminationService examinationService = null)
        {
            _diagsv = diagnosticService ?? throw new ArgumentNullException(nameof(diagnosticService));
            _exdiag = examinationService;
        }
        string IDiagnosticService.Name { get => _diagsv.Name; set => _diagsv.Name = value; }
        double IDiagnosticService.Price { get => _diagsv.Price; set => _diagsv.Price = value; }
        bool IDiagnosticService.IsNameValid { get => _diagsv.Name.IsValidName(); }
        IDoctor IDiagnosticService.Doctor
        {
            get
            {
                if (_exdiag != null)
                    throw new InvalidOperationException("This diagnostic service is only a template.");
                return _doctor;
            }
        }
        IDocument IDiagnosticService.Document { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
}