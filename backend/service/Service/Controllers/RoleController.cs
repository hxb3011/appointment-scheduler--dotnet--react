using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Requests.Create;
using AppointmentScheduler.Domain.Responses;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IRepository repository, ILogger<RoleController> logger) : ControllerBase
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<RoleController> _logger = logger;

    private RoleResponse MakeResponse(IRole role)
        => !_repository.TryGetKeyOf(role, out Role key) ? null
        : new() { Id = key.Id, Name = key.Name, Description = key.Description };

    [HttpGet]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.ReadRole])]
    public ActionResult<IEnumerable<RoleResponse>> GetPagedRoles([FromBody] PagedGetAllRequest request)
        => Ok(_repository.GetEntities<IRole>(request.Offset, request.Count, request.By).Select(MakeResponse));

    [HttpGet("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadRole])]
    public async Task<ActionResult<RoleResponse>> GetRole(uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null) return NotFound();
        return Ok(MakeResponse(role));
    }

    [HttpPost]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.CreateRole])]
    public async Task<ActionResult> CreateRole([FromBody] RoleRequest request)
    {
        var role = await _repository.ObtainEntity<IRole>();
        if (role == null)
            return BadRequest("can not create");

        role.Name = request.Name;
        if (!role.IsNameValid)
            return BadRequest("name not valid");

        if (await role.IsNameExisted())
            return BadRequest("name existed");

        role.Description = request.Description;
        if (!role.IsDescriptionValid)
            return BadRequest("description not valid");

        if (!await role.Create())
            return BadRequest("can not create");

        return Ok("success");
    }

    [HttpPut("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateRole])]
    public async Task<ActionResult> UpdateRole([FromBody] RoleRequest request, uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null) return NotFound();
        string v;
        if ((v = request.Name) != null)
        {
            role.Name = v;
            if (!role.IsNameValid)
                return BadRequest("name not valid");

            if (await role.IsNameExisted())
                return BadRequest("name existed");
        }

        if ((v = request.Description) != null)
        {
            role.Description = v;
            if (!role.IsDescriptionValid)
                return BadRequest("name not valid");
        }

        if (!await role.Update())
            return BadRequest("can not update");

        return Ok("success");
    }

    [HttpDelete("{id}")]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.DeleteRole])]
    public async Task<ActionResult> DeleteRole(uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null) return NotFound();

        if (!await role.Delete())
            return BadRequest("can not delete");

        return Ok("success");
    }

    [HttpGet("permissions")]
    [JSONWebToken(AuthenticationRequired = true)]
    public ActionResult<IEnumerable<string>> GetPermissions() => Ok(Enum.GetNames<Permission>());

    [HttpGet("{id}/permissions")]
    [JSONWebToken(RequiredPermissions = [Permission.ReadRole])]
    public async Task<ActionResult<IEnumerable<string>>> GetPermissions(uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null) return NotFound();
        return Ok(role.Permissions.Select(Enum.GetName));
    }

    [HttpPut("{id}/permission/{permid}")]
    [JSONWebToken(RequiredPermissions = [Permission.SystemPrivilege, Permission.UpdateRole])]
    public async Task<ActionResult> ChangePermission(uint id, string permid, bool granted)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null) return NotFound();

        if (!Enum.TryParse(permid, out Permission permission))
            return BadRequest("invalid permission");

        role.SetPermissionGranted(permission, granted);

        if (!await role.Update())
            return BadRequest("can not update");

        return Ok("success");
    }
}