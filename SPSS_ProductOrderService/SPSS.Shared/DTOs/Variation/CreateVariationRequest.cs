using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Variation
{
    public class CreateVariationRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.Variation.NameRequired)]
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Variation.NameTooLong)]
        public string Name { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.Variation.ProductCategoryIdRequired)]
        public Guid? ProductCategoryId { get; set; }
    }
}
