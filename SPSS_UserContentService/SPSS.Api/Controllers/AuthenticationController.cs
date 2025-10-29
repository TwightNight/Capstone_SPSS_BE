using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.Service.Services.Interfaces;
using System.Security.Claims;
using System.Security;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authService, ILogger<AuthenticationController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi xảy ra khi đăng nhập user {Username}", request.UsernameOrEmail);
            return StatusCode(500, "Lỗi hệ thống khi đăng nhập.");
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Login), new { userId }, new { userId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Lỗi đăng ký (đã rollback) cho user {Username}", request.UserName);
            return StatusCode(500, "Không thể hoàn tất đăng ký. Vui lòng thử lại.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi hệ thống khi đăng ký user {Username}", request.UserName);
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost("register-manager")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterManager([FromBody] RegisterRequest request)
    {
        try
        {
            var userId = await _authService.RegisterForManagerAsync(request);
            return CreatedAtAction(nameof(Login), new { userId }, new { userId });
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Lỗi đăng ký Manager (đã rollback) cho user {Username}", request.UserName);
            return StatusCode(500, "Không thể hoàn tất đăng ký.");
        }
    }

    [HttpPost("register-staff")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterRequest request)
    {
        try
        {
            var userId = await _authService.RegisterForStaffAsync(request);
            return CreatedAtAction(nameof(Login), new { userId }, new { userId });
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Lỗi đăng ký Staff (đã rollback) cho user {Username}", request.UserName);
            return StatusCode(500, "Không thể hoàn tất đăng ký.");
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = GetUserIdFromClaims();
            await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi đổi mật khẩu cho user {UserId}", GetUserIdFromClaims());
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var expiredAccessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(expiredAccessToken))
            {
                return Unauthorized(new { message = "Không tìm thấy Access Token." });
            }
            var response = await _authService.RefreshTokenAsync(expiredAccessToken, request.RefreshToken);
            return Ok(response);
        }
        catch (SecurityException ex) { return Unauthorized(new { message = ex.Message }); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi làm mới token.");
            return StatusCode(500, "Lỗi hệ thống.");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            await _authService.LogoutAsync(request.RefreshToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi đăng xuất.");
            return NoContent();
        }
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        try
        {
            await _authService.AssignRoleToUser(request.UserId, request.RoleName);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi gán vai trò {RoleName} cho user {UserId}", request.RoleName, request.UserId);
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