using SPSS.Shared.Constants; // Thêm using này
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Address;

public class AddressForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CountryIdIsRequired)]
    public int CountryId { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CustomerNameIsRequired)]
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.CustomerNameTooLong)]
    public string CustomerName { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberIsRequired)]
    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberTooLong)]
    [Phone(ErrorMessage = ExceptionMessageConstants.Validation.InvalidPhoneFormat)]
    public string PhoneNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.StreetNumberIsRequired)]
    [StringLength(50, ErrorMessage = ExceptionMessageConstants.Validation.StreetNumberTooLong)]
    public string StreetNumber { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.AddressLine1IsRequired)]
    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.AddressLine1TooLong)]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.AddressLine2TooLong)]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CityIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.CityTooLong)]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.WardIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.WardTooLong)]
    public string Ward { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.PostcodeIsRequired)]
    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PostcodeTooLong)]
    public string Postcode { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.ProvinceIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.ProvinceTooLong)]
    public string Province { get; set; } = null!;
}