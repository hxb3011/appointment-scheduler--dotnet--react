using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IRepository repository, ILogger<RoleController> logger) : ControllerBase
{
    private readonly IRepository _repository = repository;
    private readonly ILogger<RoleController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<RoleResponse>> GetPagedRoles([FromBody] PagedGetAllRequest request)
    {
        var db = await _repository.GetService<DbContext>();
        var query =
            from r in db.Set<Role>()
            orderby r.Name ascending
            select r;

        return Ok(
            from r in query.Skip(request.Offset).Take(request.Count)
            select new RoleResponse() { Id = r.Id, Name = r.Name, Description = r.Description }
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateRole([FromBody] CreateUpdateRoleRequest request)
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
        if (!role.IsNameValid)
            return BadRequest("name not valid");

        if (!await role.Create())
            return BadRequest("can not create");

        return Ok("success");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRole([FromBody] CreateUpdateRoleRequest request, uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null)
            return BadRequest("not found");

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
    public async Task<ActionResult> DeleteRole(uint id)
    {
        var role = await _repository.GetEntityBy<uint, IRole>(id);
        if (role == null)
            return BadRequest("not found");

        if (!await role.Delete())
            return BadRequest("can not delete");

        return Ok("success");
    }
}