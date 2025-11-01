using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Country
{
    public class CreateCountryRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Country.CountryCodeRequired)]
        [StringLength(10, ErrorMessage = ExceptionMessageConstants.Country.CountryCodeTooLong)]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Country.CountryNameRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Country.CountryNameTooLong)]
        public string CountryName { get; set; }
    }
}
