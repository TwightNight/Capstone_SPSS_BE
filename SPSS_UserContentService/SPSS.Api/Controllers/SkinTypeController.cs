using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/skin-types")]
public class SkinTypeController : ControllerBase
{
    private readonly ISkinTypeService _skinTypeService;
    private readonly ILogger<SkinTypeController> _logger;

    public SkinTypeController(ISkinTypeService skinTypeService, ILogger<SkinTypeController> logger)
    {
        _skinTypeService = skinTypeService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<SkinTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var response = await _skinTypeService.GetPagedAsync(pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy danh sách SkinType phân trang.");
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SkinTypeWithDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var skinType = await _skinTypeService.GetByIdAsync(id);
            return Ok(skinType);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tìm SkinType có ID {Id}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SkinTypeWithDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] SkinTypeForCreationDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdDto = await _skinTypeService.CreateAsync(dto);
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
            _logger.LogError(ex, "Lỗi khi tạo SkinType mới {Name}", dto.Name);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPut("{id:guid}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(SkinTypeWithDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SkinTypeForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedDto = await _skinTypeService.UpdateAsync(id, dto);
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
            _logger.LogError(ex, "Lỗi khi cập nhật SkinType {Id}", id);
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
            await _skinTypeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // Bắt lỗi "InUseBy..."
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa SkinType {Id}", id);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }
}