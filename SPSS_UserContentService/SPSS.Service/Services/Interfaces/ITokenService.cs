using SPSS.BusinessObject.Dto.Authentication;

namespace SPSS.Service.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(AuthUserDto user); string GenerateRefreshToken();
    bool ValidateAccessToken(string token, out Guid userId);
    Task<(string accessToken, string refreshToken, AuthUserDto authUserDto)> RefreshTokenAsync(string accessToken, string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}