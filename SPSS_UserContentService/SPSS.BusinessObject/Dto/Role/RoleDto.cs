using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Role;

public class RoleDto
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
}
