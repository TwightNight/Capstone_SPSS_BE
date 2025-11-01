using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.VariationOption
{
    public class CreateVariationOptionRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.VariationOption.ValueRequired)]
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.VariationOption.ValueTooLong)]
        public string Value { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.VariationOption.VariationIdRequired)]
        public Guid VariationId { get; set; }
    }
}
