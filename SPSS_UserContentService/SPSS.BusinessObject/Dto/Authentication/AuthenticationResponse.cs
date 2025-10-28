using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Authentication;

public class AuthenticationResponse : TokenResponse 
{
    public AuthUserDto AuthUserDto { get; set; }
}