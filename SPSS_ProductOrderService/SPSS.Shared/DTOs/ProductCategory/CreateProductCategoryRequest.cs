using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductCategory
{
    public class CreateProductCategoryRequest
    {
        public Guid? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string CategoryName { get; set; }
    }
}
