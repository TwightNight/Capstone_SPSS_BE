using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.SkinCondition;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
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
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _skinConditionService.GetPagedAsync(pageNumber, pageSize);
        return Ok(ApiResponse.Ok(response, "Skin conditions retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var skinCondition = await _skinConditionService.GetByIdAsync(id);
        return Ok(ApiResponse.Ok(skinCondition, "Skin condition retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SkinConditionForCreationDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var userId = GetUserIdFromClaims();
        var createdDto = await _skinConditionService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, ApiResponse.Ok(createdDto, "Skin condition created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SkinConditionForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var userId = GetUserIdFromClaims();
        var updatedDto = await _skinConditionService.UpdateAsync(id, dto, userId);
        return Ok(ApiResponse.Ok(updatedDto, "Skin condition updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserIdFromClaims();
        await _skinConditionService.DeleteAsync(id, userId);
        return Ok(ApiResponse.Ok<object>(null, "Skin condition deleted successfully."));
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