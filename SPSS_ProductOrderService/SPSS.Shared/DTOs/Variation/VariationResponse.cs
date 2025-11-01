using SPSS.Shared.DTOs.VariationOption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Variation
{
    public class VariationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public List<VariationOptionResponse> Options { get; set; } = new List<VariationOptionResponse>();
    }

}
