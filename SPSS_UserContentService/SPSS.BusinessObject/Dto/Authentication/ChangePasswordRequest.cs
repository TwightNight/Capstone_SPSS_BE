using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

/// <summary>
/// DTO chứa thông tin để người dùng tự thay đổi mật khẩu.
/// </summary>
public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(8, ErrorMessage = "The new password must be at least 8 characters long.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}