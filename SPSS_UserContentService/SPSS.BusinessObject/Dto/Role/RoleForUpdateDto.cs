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
    [Required(ErrorMessage = "Role name is required")]
    [MaxLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
    public string RoleName { get; set; }

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }
}
