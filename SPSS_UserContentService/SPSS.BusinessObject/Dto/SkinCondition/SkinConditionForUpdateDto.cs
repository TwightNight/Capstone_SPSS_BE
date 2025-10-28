using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.SkinCondition
{
	public class SkinConditionForUpdateDto
	{
		[Required]
		public Guid Id { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public int? SeverityLevel { get; set; }

		public bool? IsChronic { get; set; }
	}
}
