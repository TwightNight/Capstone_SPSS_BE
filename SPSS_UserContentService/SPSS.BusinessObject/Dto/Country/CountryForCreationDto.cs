using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.BusinessObject.Dto.Country;

public class CountryForCreationDto
{
    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CountryCodeIsRequired)]
    [StringLength(10, ErrorMessage = ExceptionMessageConstants.Validation.CountryCodeTooLong)]
    public string CountryCode { get; set; } = null!;

    [Required(ErrorMessage = ExceptionMessageConstants.Validation.CountryNameIsRequired)]
    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.CountryNameTooLong)]
    public string CountryName { get; set; } = null!;
}
