using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class LoginRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.UsernameOrEmailIsRequired)]
    public string UsernameOrEmail { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PasswordIsRequired)]
    public string Password { get; set; }
}