using SPSS.Shared.Constants; // Thêm using này
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Address;

public class AddressForUpdateDto
{
    public int CountryId { get; set; }

    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.CustomerNameTooLong)]
    public string CustomerName { get; set; } = null!;

    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PhoneNumberTooLong)]
    [Phone(ErrorMessage = ExceptionMessageConstants.Validation.InvalidPhoneFormat)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(50, ErrorMessage = ExceptionMessageConstants.Validation.StreetNumberTooLong)]
    public string StreetNumber { get; set; } = null!;

    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.AddressLine1TooLong)]
    public string AddressLine1 { get; set; } = null!;

    [StringLength(200, ErrorMessage = ExceptionMessageConstants.Validation.AddressLine2TooLong)]
    public string? AddressLine2 { get; set; }

    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.CityTooLong)]
    public string City { get; set; } = null!;

    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.WardTooLong)]
    public string Ward { get; set; } = null!;

    [StringLength(20, ErrorMessage = ExceptionMessageConstants.Validation.PostcodeTooLong)]
    public string Postcode { get; set; } = null!;

    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.ProvinceTooLong)]
    public string Province { get; set; } = null!;
}