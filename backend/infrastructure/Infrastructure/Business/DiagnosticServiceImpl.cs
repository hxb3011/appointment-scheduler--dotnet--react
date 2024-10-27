using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Infrastructure.Business
{
    internal sealed class DiagnosticServiceImpl : BaseEntity, IDiagnosticService
    {
        string IDiagnosticService.Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        double IDiagnosticService.Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IDoctor IDiagnosticService.Doctor { get => throw new NotImplementedException(); }
        IDocument IDiagnosticService.Document { get => new DocumentImpl(); set => throw new NotImplementedException(); }

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