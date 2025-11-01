using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class AssignRoleRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.UserIdIsRequired)]
    public string UserId { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.RoleNameIsRequired)]
    public string RoleName { get; set; }
}