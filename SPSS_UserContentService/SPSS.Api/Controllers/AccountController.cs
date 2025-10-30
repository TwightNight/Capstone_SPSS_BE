using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Account;
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
[Route("api/account")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, ILogger<AccountController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = GetUserIdFromClaims();
        var accountInfo = await _userService.GetAccountInfoAsync(userId);
        return Ok(ApiResponse.Ok(accountInfo, "Account information retrieved successfully."));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMyAccount([FromBody] AccountForUpdateDto dto)
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
        var updatedAccount = await _userService.UpdateAccountInfoAsync(userId, dto);
        return Ok(ApiResponse.Ok(updatedAccount, "Account updated successfully."));
    }

    [HttpPatch("avatar")]
    public async Task<IActionResult> UpdateMyAvatar([FromBody] UpdateAvatarRequest request)
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
        var oldAvatarUrl = await _userService.UpdateAvatarAsync(userId, request.AvatarUrl);
        var data = new { oldAvatar = oldAvatarUrl, newAvatar = request.AvatarUrl };
        return Ok(ApiResponse.Ok(data, "Avatar updated successfully."));
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