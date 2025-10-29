using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

/// <summary>
/// DTO dùng cho request gán vai trò cho người dùng (Admin).
/// </summary>
public class AssignRoleRequest
{
    [Required(ErrorMessage = "UserId là bắt buộc")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "RoleName là bắt buộc")]
    public string RoleName { get; set; }
}