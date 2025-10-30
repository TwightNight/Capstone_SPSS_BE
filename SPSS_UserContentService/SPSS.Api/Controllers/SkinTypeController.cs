using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.SkinType;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/skin-types")]
[Authorize(Roles = "Admin")]
public class SkinTypeController : ControllerBase
{
    private readonly ISkinTypeService _skinTypeService;
    private readonly ILogger<SkinTypeController> _logger;

    public SkinTypeController(ISkinTypeService skinTypeService, ILogger<SkinTypeController> logger)
    {
        _skinTypeService = skinTypeService ?? throw new ArgumentNullException(nameof(skinTypeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var response = await _skinTypeService.GetPagedAsync(pageNumber, pageSize);
        return Ok(ApiResponse.Ok(response, "Skin types retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var skinType = await _skinTypeService.GetByIdAsync(id);
        return Ok(ApiResponse.Ok(skinType, "Skin type retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SkinTypeForCreationDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var createdDto = await _skinTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, ApiResponse.Ok(createdDto, "Skin type created successfully."));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SkinTypeForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var updatedDto = await _skinTypeService.UpdateAsync(id, dto);
        return Ok(ApiResponse.Ok(updatedDto, "Skin type updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _skinTypeService.DeleteAsync(id);
        return Ok(ApiResponse.Ok<object>(null, "Skin type deleted successfully."));
    }
}