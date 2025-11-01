using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Country
{
    public class UpdateCountryRequest
    {
        [StringLength(10, ErrorMessage = ExceptionMessageConstants.Country.CountryCodeTooLong)]
        public string CountryCode { get; set; }

        [StringLength(100, ErrorMessage = ExceptionMessageConstants.Country.CountryNameTooLong)]
        public string CountryName { get; set; }
    }
}
