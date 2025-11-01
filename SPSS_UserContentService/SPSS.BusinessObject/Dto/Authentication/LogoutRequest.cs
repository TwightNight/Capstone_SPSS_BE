using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class LogoutRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.RefreshTokenIsRequired)]
    public string RefreshToken { get; set; }
}