using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.VariationOption
{
    public class VariationOptionResponse
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid VariationId { get; set; }
    }
}
