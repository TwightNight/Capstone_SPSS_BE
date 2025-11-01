using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductStatus
{
    public class UpdateProductStatusRequest
    {
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.ProductStatus.StatusNameTooLong)]
        public string StatusName { get; set; }

        [StringLength(500, ErrorMessage = ExceptionMessageConstants.ProductStatus.DescriptionTooLong)]
        public string Description { get; set; }
    }
}
