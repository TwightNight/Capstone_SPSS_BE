using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Address;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/addresses")]
[Authorize]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService ?? throw new ArgumentNullException(nameof(addressService));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<AddressDto>), 200)]
    public async Task<IActionResult> GetMyAddresses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserIdFromClaims();
        var pagedResponse = await _addressService.GetByUserIdPagedAsync(userId, pageNumber, pageSize);
        return Ok(pagedResponse);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddressDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto createDto)
    {
        var userId = GetUserIdFromClaims();
        var createdAddress = await _addressService.CreateAsync(createDto, userId);
        return CreatedAtAction(nameof(GetMyAddresses), new { id = createdAddress.Id }, createdAddress);
    }

    [HttpPut("{addressId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] AddressForUpdateDto updateDto)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.UpdateAsync(addressId, updateDto, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{addressId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAddress(Guid addressId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.DeleteAsync(addressId, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{addressId:guid}/set-default")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> SetDefaultAddress(Guid addressId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            await _addressService.SetAsDefaultAsync(addressId, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            throw new InvalidOperationException("User identifier is missing or invalid in the token.");
        }
        return userId;
    }
}