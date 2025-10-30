using SPSS.BusinessObject.Dto.Authentication;

namespace SPSS.Service.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest);
    Task<AuthenticationResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<AuthUserDto> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthUserDto> RegisterPrivilegedUserAsync(PrivilegedRegisterRequest registerRequest);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task AssignRoleToUser(string userId, string roleName);
    Task SendVerificationOtpAsync(Guid userId, string email);     // used after register to send OTP
    Task VerifyAccountByOtpAsync(string email, string code);      // already referenced in controller
    Task ResendVerificationOtpAsync(string email);                // new: resend flow, uses internal checks
}