using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Address;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Errors;
using SPSS.Shared.Exceptions;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Security;
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
    public async Task<IActionResult> GetMyAddresses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserIdFromClaims();
        var pagedResponse = await _addressService.GetByUserIdPagedAsync(userId, pageNumber, pageSize);
        return Ok(pagedResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto createDto)
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
        var createdAddress = await _addressService.CreateAsync(createDto, userId);
        return CreatedAtAction(nameof(GetMyAddresses), new { id = createdAddress.Id }, createdAddress);
    }

    [HttpPut("{addressId:guid}")]
    public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] AddressForUpdateDto updateDto)
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
        await _addressService.UpdateAsync(addressId, updateDto, userId);
        return NoContent();
    }

    [HttpDelete("{addressId:guid}")]
    [Authorize(Roles = "Admin")]
	public async Task<IActionResult> DeleteAddress(Guid addressId)
    {
        var userId = GetUserIdFromClaims();
        await _addressService.DeleteAsync(addressId, userId);
        return NoContent();
    }

    //[HttpPatch("{addressId:guid}/set-default")]
    //public async Task<IActionResult> SetDefaultAddress(Guid addressId)
    //{
    //    var userId = GetUserIdFromClaims();
    //    await _addressService.SetAsDefaultAsync(addressId, userId);
    //    return NoContent();
    //}

    private Guid GetUserIdFromClaims()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            // Ném SecurityException để middleware xử lý thành lỗi 401 Unauthorized.
            throw new SecurityException("User identifier is missing or invalid in the security token.");
        }
        return userId;
    }
}