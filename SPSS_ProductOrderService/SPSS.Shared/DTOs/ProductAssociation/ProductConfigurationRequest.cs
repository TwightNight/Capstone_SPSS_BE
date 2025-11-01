using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductAssociation
{
    public class ProductConfigurationRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.ProductAssociation.ProductIdRequired)]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.ProductAssociation.VariationOptionIdRequired)]
        public Guid VariationOptionId { get; set; }
    }
}
