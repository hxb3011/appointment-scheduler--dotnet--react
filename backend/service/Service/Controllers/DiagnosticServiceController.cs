﻿using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Requests.Create;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosticServiceController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly ILogger<DiagnosticServiceController> _logger;

    public DiagnosticServiceController(IRepository repository, ILogger<DiagnosticServiceController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private DiagnosticServiceResponse MakeResponse(IDiagnosticService role)
        => !_repository.TryGetKeyOf(role, out DiagnosticService key) ? null
        : new() { Id = key.Id, Name = key.Name, Price = key.Price };

    [HttpGet]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadDiagnosticService])]
    public ActionResult<IEnumerable<DiagnosticServiceResponse>> GetPagedDiagnosticServices([FromBody] PagedGetAllRequest request)
        => Ok(_repository.GetEntities<IDiagnosticService>(request.Offset, request.Count, request.By).Select(MakeResponse));

    [HttpGet("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadDiagnosticService])]
    public async Task<ActionResult<DiagnosticServiceResponse>> GetDiagnosticService(uint id)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
        if (diagnosticService == null) return NotFound();
        return Ok(MakeResponse(diagnosticService));
    }

    [HttpPost]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.CreateDiagnosticService])]
    public async Task<ActionResult> CreateDiagnosticService([FromBody] DiagnosticServiceRequest request)
    {
        var diagnosticService = await _repository.ObtainEntity<IDiagnosticService>();
        if (diagnosticService == null)
            return BadRequest("can not create");

        diagnosticService.Name = request.Name;
        if (!diagnosticService.IsNameValid)
            return BadRequest("name not valid");

        diagnosticService.Price = request.Price;

        if (!await diagnosticService.Create())
            return BadRequest("can not create");

        return Ok("success");
    }

    [HttpPut("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateDiagnosticService])]
    public async Task<ActionResult> UpdateDiagnosticService([FromBody] DiagnosticServiceRequest request, uint id)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
        if (diagnosticService == null) return NotFound();
        string v;
        if ((v = request.Name) != null)
        {
            diagnosticService.Name = v;
            if (!diagnosticService.IsNameValid)
                return BadRequest("name not valid");
        }

        diagnosticService.Price = request.Price;

        if (!await diagnosticService.Update())
            return BadRequest("can not update");

        return Ok("success");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.DeleteDiagnosticService])]
    public async Task<ActionResult> DeleteDiagnosticService(uint id)
    {
        var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
        if (diagnosticService == null) return NotFound();

        if (!await diagnosticService.Delete())
            return BadRequest("can not delete");

        return Ok("success");
    }




    // private ExaminationDiagnosticResponse MakeExaminationDiagnosticResponse(IDiagnosticService role)
    //     => !_repository.TryGetKeyOf(role, out ExaminationService key) ? null
    //     : new() { DoctorId = key.DoctorId, DiagnosticServiceId = key.DiagnosticServiceId, ExaminationId = key.ExaminationId };


    // [HttpGet]
    // [JSONWebToken(RequiredPermissions = [Permission.ReadDiagnosticService])]
    // public async Task<ActionResult<IEnumerable<ExaminationDiagnosticResponse>>> GetPagedExaminationDiagnostics([FromBody] PagedGetAllRequest request, uint examination)
    // {
    //     var ex = await _repository.GetEntityBy<uint, IExamination>(examination);
    //     if (ex == null) return NotFound("examination not found");
    //     var query = ex.DiagnosticServices.AsQueryable();
    //     var param = Expression.Parameter(typeof(IDiagnosticService));
    //     return Ok(query.OrderBy(Expression.Lambda<Func<IDiagnosticService, object>>(
    //         Expression.Property(param, request.By ?? nameof(IDiagnosticService.Name)), param
    //     )).Skip(request.Offset).Take(request.Count).Select(MakeExaminationDiagnosticResponse));
    // }

    // [HttpGet("{id}")]
    // [JSONWebToken(RequiredPermissions = [Permission.ReadDiagnosticService])]
    // public async Task<ActionResult> GetExaminationDiagnostic(uint id, uint examination)
    // {
    //     var ex = await _repository.GetEntityBy<uint, IExamination>(examination);
    //     if (ex == null) return NotFound("examination not found");

    //     var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
    //     if (diagnosticService == null) return NotFound();
    //     return Ok(MakeResponse(diagnosticService));
    // }

    // [HttpGet("{id}/document")]
    // [JSONWebToken(RequiredPermissions = [Permission.ReadDiagnosticService])]
    // public async Task<ActionResult> GetExaminationDiagnosticDocument(uint id, uint examination)
    // {
    //     var ex = await _repository.GetEntityBy<uint, IExamination>(examination);
    //     if (ex == null) return NotFound("examination not found");

    //     var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
    //     if (diagnosticService == null) return NotFound();
    //     return Ok(MakeResponse(diagnosticService));
    // }

    // [HttpPost("{id}")]
    // [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.CreateDiagnosticService])]
    // public async Task<ActionResult> CreateExaminationDiagnostic(uint id, uint examination, uint doctor)
    // {
    //     var ex = await _repository.GetEntityBy<uint, IExamination>(examination);
    //     if (ex == null) return NotFound("examination not found");
    //     var ds = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
    //     if (ds == null) return NotFound("diagnosticService not found");
    //     var d = await _repository.GetEntityBy<uint, IDoctor>(doctor);
    //     if (d == null) return NotFound("doctor not found");

    //     var examinationDiagnostic = await ex.ObtainDiagnostic(d, ds);
    //     if (examinationDiagnostic == null)
    //         return BadRequest("can not create");

    //     if (!await examinationDiagnostic.Create())
    //         return BadRequest("can not create");

    //     return Ok("success");
    // }

    // [HttpPost]
    // [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.SealExaminationDiagnostic])]
    // public async Task<ActionResult> UploadExaminationDiagnosticDocument(IFormFile file, uint examination)
    // {
    //     var diagnosticService = await _repository.ObtainEntity<IDiagnosticService>();
    //     if (diagnosticService == null)
    //         return BadRequest("can not create");

    //     diagnosticService.Name = request.Name;
    //     if (!diagnosticService.IsNameValid)
    //         return BadRequest("name not valid");

    //     diagnosticService.Price = request.Price;

    //     if (!await diagnosticService.Create())
    //         return BadRequest("can not create");

    //     return Ok("success");
    // }

    // [HttpDelete("{id}")]
    // [JSONWebToken(RequiredPermissions = [Permission.DeleteExaminationDiagnostic])]
    // public async Task<ActionResult> DeleteExaminationDiagnostic(uint id, uint examination)
    // {
    //     var diagnosticService = await _repository.GetEntityBy<uint, IDiagnosticService>(id);
    //     if (diagnosticService == null) return NotFound();

    //     if (!await diagnosticService.Delete())
    //         return BadRequest("can not delete");

    //     return Ok("success");
    // }
}
