using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.VariationOption
{
    public class UpdateVariationOptionRequest
    {
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.VariationOption.ValueTooLong)]
        public string Value { get; set; }
    }
}
