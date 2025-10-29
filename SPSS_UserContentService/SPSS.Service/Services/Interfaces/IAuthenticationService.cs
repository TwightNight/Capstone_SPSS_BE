using SPSS.BusinessObject.Dto.Authentication;

namespace SPSS.Service.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> LoginAsync(LoginRequest loginRequest);
    Task<AuthenticationResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<AuthUserDto> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthUserDto> RegisterForManagerAsync(RegisterRequest registerRequest);
    Task<AuthUserDto> RegisterForStaffAsync(RegisterRequest registerRequest);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task AssignRoleToUser(string userId, string roleName);
}