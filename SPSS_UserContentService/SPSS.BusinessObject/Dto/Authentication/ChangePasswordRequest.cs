using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class ChangePasswordRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CurrentPasswordIsRequired)]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.NewPasswordIsRequired)]
    [MinLength(8, ErrorMessage = ExceptionMessageConstants.Validation.NewPasswordMinLength)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}