using System;
using System.ComponentModel.DataAnnotations;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.ProductCategory
{
    public class CreateProductCategoryRequest
    {
        public Guid? ParentCategoryId { get; set; }

        [Required(ErrorMessage = ExceptionMessageConstants.ProductCategory.CategoryNameRequired)]
        [StringLength(100, ErrorMessage = ExceptionMessageConstants.ProductCategory.CategoryNameTooLong)]
        public string CategoryName { get; set; }
    }
}
