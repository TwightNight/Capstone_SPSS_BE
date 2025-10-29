using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

/// <summary>
/// DTO chứa refresh token cần bị thu hồi khi đăng xuất.
/// </summary>
public class LogoutRequest
{
    [Required(ErrorMessage = "Refresh token là bắt buộc")]
    public string RefreshToken { get; set; }
}