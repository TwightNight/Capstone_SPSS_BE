using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.SkinCondition
{
	public class SkinConditionForCreationDto
	{
		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		[StringLength(1000)]
		public string Description { get; set; }

		public int? SeverityLevel { get; set; }

		public bool? IsChronic { get; set; }
	}
}
