using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductCategory
{
    public class UpdateProductCategoryRequest
    {
        public Guid? ParentCategoryId { get; set; }

        [StringLength(100, ErrorMessage = ExceptionMessageConstants.ProductCategory.CategoryNameTooLong)]
        public string CategoryName { get; set; }
    }
}
