using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.SkinCondition
{
    public class SkinConditionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SeverityLevel { get; set; }
        public bool? IsChronic { get; set; }
    }   
}
