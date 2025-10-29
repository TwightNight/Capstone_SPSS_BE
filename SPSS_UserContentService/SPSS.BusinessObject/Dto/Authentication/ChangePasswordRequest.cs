using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

/// <summary>
/// DTO chứa thông tin để người dùng tự thay đổi mật khẩu.
/// </summary>
public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
    public string NewPassword { get; set; }
}