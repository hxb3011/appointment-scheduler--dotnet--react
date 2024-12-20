using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;

namespace AppointmentScheduler.Infrastructure.Business;

internal sealed class DiagnosticServiceImpl : BaseEntity, IDiagnosticService
{
    internal DiagnosticService _diagsv;
    internal ExaminationService _exdiag;
    private IDoctor _doctor;
    private IResourceManagerService _resourceManager;
    internal DiagnosticServiceImpl(DiagnosticService diagnosticService, ExaminationService examinationService = null, IDoctor doctor = null)
    {
        _diagsv = diagnosticService ?? throw new ArgumentNullException(nameof(diagnosticService));
        _exdiag = examinationService;
        _doctor = doctor;
        ((IBehavioralEntity)this).Deleted += DeleteDocument;
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

    Stream IDiagnosticService.Document(bool readOnly) => _exdiag == null ? null
        : _resourceManager.Resource<DiagnosticServiceImpl>(_exdiag.Id.ToString(), readOnly);

    private void DeleteDocument(object sender, EventArgs e)
    {
        if (_exdiag == null) return;
        _resourceManager.RemoveResource<DiagnosticServiceImpl>(_exdiag.Id.ToString());
    }

    protected override Task<bool> Create()
    {
        // TODO: Process Document
        if (_exdiag != null)
        {
            _dbContext.Add(_exdiag);
            _dbContext.Update(_diagsv);
        }
        else _dbContext.Add(_diagsv);
        return Task.FromResult(true);
    }

    protected override Task<bool> Delete()
    {
        // TODO: Process Document
        if (_exdiag != null)
        {
            _dbContext.Remove(_exdiag);
            _dbContext.Update(_diagsv);
        }
        else _dbContext.Remove(_diagsv);
        return Task.FromResult(true);
    }

    protected override async Task<bool> Initilize()
    {
        _resourceManager = await _repository.GetService<IResourceManagerService>();
        return true;
    }

    protected override Task<bool> Update()
    {
        // TODO: Process Document
        if (_exdiag != null)
        {
            _dbContext.Update(_exdiag);
            _dbContext.Update(_diagsv);
        }
        else _dbContext.Update(_diagsv);
        return Task.FromResult(true);
    }
}