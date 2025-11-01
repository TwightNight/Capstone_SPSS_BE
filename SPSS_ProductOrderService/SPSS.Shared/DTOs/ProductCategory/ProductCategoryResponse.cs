using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.ProductCategory
{
    public class ProductCategoryResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
