using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

/// <summary>
/// DTO dùng cho request gán vai trò cho người dùng (Admin).
/// </summary>
public class AssignRoleRequest
{
    [Required(ErrorMessage = "User ID is required.")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Role name is required.")]
    public string RoleName { get; set; }
}