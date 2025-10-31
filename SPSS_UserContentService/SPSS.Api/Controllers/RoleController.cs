using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Role;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetRolesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _roleService.GetPagedAsync(pageNumber, pageSize);
        return Ok(ApiResponse.Ok(response, "Roles retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var role = await _roleService.GetByIdAsync(id);
        return Ok(ApiResponse.Ok(role, "Role retrieved successfully."));
    }

    [HttpGet("name/{roleName}")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        var role = await _roleService.GetByNameAsync(roleName);
        return Ok(ApiResponse.Ok(role, "Role retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleForCreationDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var createdRole = await _roleService.CreateAsync(roleDto);
        return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, ApiResponse.Ok(createdRole, "Role created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleForUpdateDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var updatedRole = await _roleService.UpdateAsync(id, roleDto);
        return Ok(ApiResponse.Ok(updatedRole, "Role updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        await _roleService.DeleteAsync(id);
        return Ok(ApiResponse.Ok<object>(null, "Role deleted successfully."));
    }
}