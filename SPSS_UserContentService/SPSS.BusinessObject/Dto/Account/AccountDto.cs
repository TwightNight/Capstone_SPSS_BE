using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Account;

public class AccountDto
{
    public Guid UserId { get; set; }
    public string? SkinType { get; set; } 
    public string? UserName { get; set; }
    public string? SurName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
}
