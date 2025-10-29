using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Account;
using SPSS.Service.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Security.Claims;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/account")]
//[Authorize] // BẮT BUỘC: Người dùng phải đăng nhập
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, ILogger<AccountController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyAccountInfo()
    {
        try
        {
            var userId = GetUserIdFromClaims();
            var accountInfo = await _userService.GetAccountInfoAsync(userId);
            return Ok(accountInfo);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy thông tin account cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPut("me")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateMyAccountInfo([FromBody] AccountForUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
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
        catch (InvalidOperationException ex) // Bắt lỗi Trùng lặp
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật account cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPatch("avatar")] // Dùng PATCH vì chỉ cập nhật 1 phần nhỏ (avatar)
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyAvatar([FromBody] UpdateAvatarRequest request)
    {
        if (string.IsNullOrEmpty(request.AvatarUrl))
        {
            return BadRequest(new { message = "Avatar URL là bắt buộc." });
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật avatar cho user {UserId}", GetUserIdFromClaims());
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
