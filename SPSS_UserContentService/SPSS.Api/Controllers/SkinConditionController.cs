using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Errors;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/skin-conditions")]
[Authorize(Roles = "Admin")]
public class SkinConditionController : ControllerBase
{
    private readonly ISkinConditionService _skinConditionService;
    private readonly ILogger<SkinConditionController> _logger;

    public SkinConditionController(ISkinConditionService skinConditionService, ILogger<SkinConditionController> logger)
    {
        _skinConditionService = skinConditionService ?? throw new ArgumentNullException(nameof(skinConditionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResponse<SkinConditionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _skinConditionService.GetPagedAsync(pageNumber, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var skinCondition = await _skinConditionService.GetByIdAsync(id);
        return Ok(skinCondition);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] SkinConditionForCreationDto dto)
    {
        var userId = GetUserIdFromClaims();
        var createdDto = await _skinConditionService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SkinConditionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SkinConditionForUpdateDto dto)
    {
        var userId = GetUserIdFromClaims();
        var updatedDto = await _skinConditionService.UpdateAsync(id, dto, userId);
        return Ok(updatedDto);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)] // Hoặc 400 tùy vào ý nghĩa của InvalidOperationException
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserIdFromClaims();
        await _skinConditionService.DeleteAsync(id, userId);
        return NoContent();
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new SecurityException("User identifier is missing or invalid in the security token.");
        }
        return userId;
    }
}