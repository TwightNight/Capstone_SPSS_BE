using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Product
{
    public class FullCreateProductRequest : CreateProductRequest
    {
        public List<Guid> SkinConditionIds { get; set; } = new List<Guid>();
        public List<Guid> SkinTypeIds { get; set; } = new List<Guid>();
        public List<Guid> VariationOptionIds { get; set; } = new List<Guid>();
    }
}
