using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Authentication;

public class AuthUserDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string EmailAddress { get; set; }
    public string Role { get; set; }
}