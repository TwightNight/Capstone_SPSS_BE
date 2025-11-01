using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Role;

public class RoleForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.RoleNameIsRequired)]
    [MaxLength(100, ErrorMessage = ExceptionMessageConstants.Validation.RoleNameTooLong)]
    public string RoleName { get; set; }

    [MaxLength(500, ErrorMessage = ExceptionMessageConstants.Validation.DescriptionTooLong500)]
    public string Description { get; set; }
}
