using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.Country;

public class CountryForUpdateDto
{
    [StringLength(10, ErrorMessage = ExceptionMessageConstants.Validation.CountryCodeTooLong)]
    public string CountryCode { get; set; } = null!;

    [StringLength(100, ErrorMessage = ExceptionMessageConstants.Validation.CountryNameTooLong)]
    public string CountryName { get; set; } = null!;
}