using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductCategory
{
    public class UpdateProductCategoryRequest
    {
        public Guid? ParentCategoryId { get; set; }

        [StringLength(100, ErrorMessage = "The category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }
    }
}
