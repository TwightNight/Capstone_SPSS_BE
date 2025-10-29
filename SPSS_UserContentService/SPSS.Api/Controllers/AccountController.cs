using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Account;
using SPSS.Service.Services.Interfaces;
using System;
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
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyAccount()
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var accountInfo = await _userService.GetAccountInfoAsync(userId);
            return Ok(accountInfo);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Account info not found for the current user.");
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateMyAccount([FromBody] AccountForUpdateDto dto)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var updatedAccount = await _userService.UpdateAccountInfoAsync(userId, dto);
            return Ok(updatedAccount);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPatch("avatar")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyAvatar([FromBody] UpdateAvatarRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.AvatarUrl))
        {
            return BadRequest(new { message = "The AvatarUrl field is required." });
        }

        try
        {
            var userId = GetUserIdFromClaims();
            var oldAvatarUrl = await _userService.UpdateAvatarAsync(userId, request.AvatarUrl);
            return Ok(new { oldAvatar = oldAvatarUrl, newAvatar = request.AvatarUrl });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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