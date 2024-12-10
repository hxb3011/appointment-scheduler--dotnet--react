using AppointmentScheduler.Domain.Requests;
using AppointmentScheduler.Presentation.Models;
using AppointmentScheduler.Presentation.Models.Enums;
using AppointmentScheduler.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Presentation.Controllers;

public class RoleController : Controller
{
	private readonly RoleService _roleService;
	private readonly ILogger<RoleService> _logger;

	public RoleController(RoleService roleService, ILogger<RoleService> logger)
	{
		_roleService = roleService;
		_logger = logger;
	}

	public PagedGetAllRequest GetPage()
	{
		PagedGetAllRequest pagedGetAllRequest = new PagedGetAllRequest
		{
			Offset = 0,
			Count = 1000
		};

		return pagedGetAllRequest;
	}

	public async Task<IActionResult> Index(int offset = 0, int count = 1000)
	 	=> View(await _roleService.GetPagedRoles(new()
		 {
			 Offset = offset,
			 Count = count
		 }));

	public async Task<IActionResult> RoleInfo(uint id)
	 	=> Ok(await _roleService.GetRoleById(id));

	public async Task<IActionResult> Create()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(RoleModel role)
	{
		if (ModelState.IsValid)
		{
			var resultMessage = await _roleService.CreateRole(new()
			{
				Name = role.Name,
				Description = role.Description
			});

			if (resultMessage == "success")
			{
				TempData["Success"] = "Thêm mới vai trò thành công";
				return RedirectToAction(nameof(Index));
			}
		}

		TempData["Error"] = "Lỗi không thể thêm vai trò này";
		return View(role);
	}

	public async Task<IActionResult> Edit(uint id)
	{
		var role = await _roleService.GetRoleById(id);
		if (role == null)
		{
			TempData["Error"] = "Đã xảy ra lỗi khi truy cập vai trò này";
			return View("Error");
		}
		RoleModel model = new()
		{
			Id = id,
			Name = role.Name,
			Description = role.Description
		};
		var perms = model.Permissions;
		foreach (var perm in await _roleService.GetPermissions())
			perms[perm] = false;
		foreach (var perm in await _roleService.GetPermissions(id))
			perms[perm] = true;
		model.IsDefault = await _roleService.CheckDefaultRole(id);
		_logger?.LogInformation("model.IsDefault = {0}", model.IsDefault);
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> SetDefaultRole(uint roleid)
	{
		_logger?.LogInformation("SetDefaultRole Entered: {0}", roleid);
		if (await _roleService.UpdateDefaultRole(roleid))
			return Json(new { success = true, message = "Cập nhật thành công" });
		return Json(new { success = false, message = "Cập nhật thất bại" });
	}

	[HttpPost]
	public async Task<IActionResult> ChangePermission(uint roleid, string permid, bool isChecked)
	{
		if (Enum.TryParse(permid, out Permission permission)
				&& "success" == await _roleService.ChangePermission(roleid, permission, isChecked))
			return Json(new { success = true, message = "Cấp quyền thành công" });
		return Json(new { success = false, message = "Cấp quyền không thành công" });
	}

	[HttpPost]
	public async Task<IActionResult> Edit(RoleModel role)
	{
		if (ModelState.IsValid)
		{
			var resultMessage = await _roleService.UpdateRole(new()
			{
				Name = role.Name,
				Description = role.Description
			}, role.Id);

			if (resultMessage == "success")
			{
				TempData["Success"] = "Chỉnh sửa vai trò thành công";
				return RedirectToAction("Index");
			}
		}

		var perms = role.Permissions;
		foreach (var perm in await _roleService.GetPermissions())
			perms[perm] = false;
		foreach (var perm in await _roleService.GetPermissions(role.Id))
			perms[perm] = true;
		role.IsDefault = await _roleService.CheckDefaultRole(role.Id);
		TempData["Error"] = "Lỗi không thể sửa vai trò này";
		return View(role);
	}

	[HttpPost]
	public async Task<IActionResult> Delete(uint id)
	{
		if ("success" != await _roleService.DeleteRole(id))
		{
			//TempData["Error"] = "Không thể xóa vai trò";
            return BadRequest("Không thể xóa vai trò");
        }
		//TempData["Success"] = "Xóa vai trò thành công";
        return Ok("Xóa vai trò thành công");
	}
}
