using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Role;

// DTO để cập nhật (Update) Role
public class RoleForUpdateDto
{
    [MaxLength(100, ErrorMessage = ExceptionMessageConstants.Validation.RoleNameTooLong)]
    public string RoleName { get; set; }

    [MaxLength(500, ErrorMessage = ExceptionMessageConstants.Validation.DescriptionTooLong500)]
    public string Description { get; set; }
}
