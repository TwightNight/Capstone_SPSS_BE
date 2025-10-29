using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Role;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/roles")]
//[Authorize(Roles = "Admin")] // BẮT BUỘC: Chỉ Admin mới được quản lý Role
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRolesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var response = await _roleService.GetPagedAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách Role phân trang.");
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        try
        {
            var role = await _roleService.GetByIdAsync(id);
            return Ok(role);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tìm Role có ID {RoleId}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("name/{roleName}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {
        try
        {
            var role = await _roleService.GetByNameAsync(roleName);
            return Ok(role);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tìm Role có tên {RoleName}", roleName);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRole([FromBody] RoleForCreationDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdRole = await _roleService.CreateAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, createdRole);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi trùng tên
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo Role mới {RoleName}", roleDto.RoleName);
            return StatusCode(500, "Lỗi hệ thống khi tạo Role.");
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleForUpdateDto roleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedRole = await _roleService.UpdateAsync(id, roleDto);
            return Ok(updatedRole);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi trùng tên
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật Role {RoleId}", id);
            return StatusCode(500, "Lỗi hệ thống khi cập nhật Role.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        try
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi "Role in use"
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa Role {RoleId}", id);
            return StatusCode(500, "Lỗi hệ thống khi xóa Role.");
        }
    }
}