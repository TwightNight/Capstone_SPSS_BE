using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductStatus
{
    public class CreateProductStatusRequest
    {
        [Required(ErrorMessage = ExceptionMessageConstants.ProductStatus.StatusNameRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.ProductStatus.StatusNameTooLong)]
        public string StatusName { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.ProductStatus.DescriptionRequired)]
        [StringLength(500, ErrorMessage = ExceptionMessageConstants.ProductStatus.DescriptionTooLong)]
        public string Description { get; set; }
    }
}
