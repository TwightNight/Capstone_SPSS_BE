using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Authentication;

public class RegisterRequest
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.UsernameIsRequiredForRegister)]
    [MinLength(3, ErrorMessage = ExceptionMessageConstants.Validation.UsernameMinLength)]
    public string UserName { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.EmailIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.EmailTooLong)]
    [EmailAddress(ErrorMessage = ExceptionMessageConstants.Validation.InvalidEmailFormat)]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberIsRequired)]
    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberTooLong)]
    [Phone(ErrorMessage = ExceptionMessageConstants.Validation.InvalidPhoneFormat)]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ExceptionMessageConstants.Validation.PasswordMinLength)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}