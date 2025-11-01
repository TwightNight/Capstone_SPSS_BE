using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Variation
{
    public class UpdateVariationRequest
    {
        [StringLength(255, ErrorMessage = ExceptionMessageConstants.Variation.NameTooLong)]
        public string Name { get; set; }

        public Guid? ProductCategoryId { get; set; }
    }
}
