using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSS.BusinessObject.Dto.Authentication;
using SPSS.BusinessObject.Dto.VerifyOtp;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Errors;
using SPSS.Shared.Exceptions;
using System;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPSS.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authService, ILogger<AuthenticationController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {

            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ValidationException(string.Join(" | ", errorMessages));
        }
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		var user = await _authService.RegisterAsync(request);
        return StatusCode(StatusCodes.Status201Created, new { user });
    }

    [HttpPost("register-privileged")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> RegisterPrivilegedUser([FromBody] PrivilegedRegisterRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		var user = new AuthUserDto();
        switch (request.RoleName)
        {
            case "Manager":
                if (!User.IsInRole("Admin"))
                {
                    return Forbid(); 
                }
                user = await _authService.RegisterForManagerAsync(request);
                break;

            case "Staff":
                user = await _authService.RegisterForStaffAsync(request);
                break;

            default:
                return BadRequest(new { message = "Invalid or unsupported role specified." });
        }
        return StatusCode(StatusCodes.Status201Created, new { user });
    }

    [HttpPost("change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
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
        await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
        return NoContent();
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		var expiredAccessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var response = await _authService.RefreshTokenAsync(expiredAccessToken, request.RefreshToken);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		await _authService.LogoutAsync(request.RefreshToken);
        return NoContent();
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        await _authService.AssignRoleToUser(request.UserId, request.RoleName);
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

    [HttpPost("verify-account")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyAccount([FromBody] VerifyOtpRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		await _authService.VerifyAccountByOtpAsync(request.Email, request.Code);
        return NoContent();
    }

    // NOW: controller delegates resend entirely to service
    [HttpPost("resend-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest request)
    {
		if (!ModelState.IsValid)
		{

			var errorMessages = ModelState.Values
				.SelectMany(v => v.Errors)
				.Select(e => e.ErrorMessage)
				.ToList();

			throw new ValidationException(string.Join(" | ", errorMessages));
		}
		await _authService.ResendVerificationOtpAsync(request.Email);
        return NoContent();
    }

}