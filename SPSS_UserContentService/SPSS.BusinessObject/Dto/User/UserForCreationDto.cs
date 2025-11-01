using SPSS.Shared.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.User;

public class UserForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.RoleIdIsRequired)]
    public Guid? RoleId { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.StatusIsRequired)]
    public string Status { get; set; }

    // Login Credentials
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.UsernameIsRequiredForRegister)]
    [MinLength(3, ErrorMessage = ExceptionMessageConstants.Validation.UsernameMinLength)]
    public string UserName { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ExceptionMessageConstants.Validation.PasswordMinLength)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    // Basic Information
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.SurNameTooLong)]
    public string SurName { get; set; }

    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.LastNameTooLong)]
    public string LastName { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.EmailIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.EmailTooLong)]
    [EmailAddress(ErrorMessage = ExceptionMessageConstants.Validation.InvalidEmailFormat)]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberIsRequired)]
    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberTooLong)]
    [Phone(ErrorMessage = ExceptionMessageConstants.Validation.InvalidPhoneFormat)]
    public string PhoneNumber { get; set; }

    [StringLength(500, ErrorMessage = ExceptionMessageConstants.Validation.AvatarUrlTooLong)]
    [Url(ErrorMessage = ExceptionMessageConstants.Validation.InvalidUrlFormat)]
    public string? AvatarUrl { get; set; }

    // Personal/Dermatological Information (optional)
    public Guid? SkinTypeId { get; set; }
    public Guid? SkinConditionId { get; set; }

    [Range(1, 150, ErrorMessage = ExceptionMessageConstants.Validation.InvalidAgeRange)]
    public int? Age { get; set; }

    public DateTime? DoB { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.DietTooLong)]
    public string? Diet { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.DailyRoutineTooLong)]
    public string? DailyRoutine { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.AllergyTooLong)]
    public string? Allergy { get; set; }

    [StringLength(1000, ErrorMessage = ExceptionMessageConstants.Validation.CertificateTooLong)]
    public string? Certificate { get; set; }
}