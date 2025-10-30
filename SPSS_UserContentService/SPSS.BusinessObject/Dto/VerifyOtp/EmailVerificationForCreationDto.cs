using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.VerifyOtp;

public class EmailVerificationForCreationDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string CodeHash { get; set; }
    public string Salt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
