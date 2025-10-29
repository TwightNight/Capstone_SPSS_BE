using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses; // Cần cho PagedResponse
using System.Security.Claims;
using System.Security;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/skin-conditions")]
public class SkinConditionController : ControllerBase
{
    private readonly ISkinConditionService _skinConditionService;
    private readonly ILogger<SkinConditionController> _logger;

    public SkinConditionController(ISkinConditionService skinConditionService, ILogger<SkinConditionController> logger)
    {
        _skinConditionService = skinConditionService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<SkinConditionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var response = await _skinConditionService.GetPagedAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách SkinCondition phân trang.");
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var skinCondition = await _skinConditionService.GetByIdAsync(id);
            return Ok(skinCondition);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tìm SkinCondition có ID {Id}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] SkinConditionForCreationDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var createdDto = await _skinConditionService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi NameAlreadyExists
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo SkinCondition mới {Name}", dto.Name);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPut("{id:guid}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SkinConditionForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var updatedDto = await _skinConditionService.UpdateAsync(id, dto, userId);
            return Ok(updatedDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi NameAlreadyExists
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật SkinCondition {Id}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpDelete("{id:guid}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _skinConditionService.DeleteAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi "InUseByUser"
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa SkinCondition {Id}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Không thể tìm thấy hoặc phân tích UserId từ claims.");
            throw new SecurityException("Thông tin người dùng không hợp lệ.");
        }

        return userId;
    }
}