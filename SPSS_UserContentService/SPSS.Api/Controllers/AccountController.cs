using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Account;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Errors;
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
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = GetUserIdFromClaims();
        var accountInfo = await _userService.GetAccountInfoAsync(userId);
        return Ok(accountInfo);
    }

    [HttpPut]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateMyAccount([FromBody] AccountForUpdateDto dto)
    {
        var userId = GetUserIdFromClaims();
        var updatedAccount = await _userService.UpdateAccountInfoAsync(userId, dto);
        return Ok(updatedAccount);
    }

    [HttpPatch("avatar")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyAvatar([FromBody] UpdateAvatarRequest request)
    {
        var userId = GetUserIdFromClaims();
        var oldAvatarUrl = await _userService.UpdateAvatarAsync(userId, request.AvatarUrl);
        return Ok(new { oldAvatar = oldAvatarUrl, newAvatar = request.AvatarUrl });
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