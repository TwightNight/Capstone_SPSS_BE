using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.Dto.SkinCondition
{
	public class SkinConditionDto
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		[StringLength(1000)]
		public string Description { get; set; }

		public int? SeverityLevel { get; set; }

		public bool? IsChronic { get; set; }

		[StringLength(100)]
		public string CreatedBy { get; set; }

		[StringLength(100)]
		public string LastUpdatedBy { get; set; }

		[StringLength(100)]
		public string DeletedBy { get; set; }

		public DateTimeOffset? CreatedTime { get; set; }

		public DateTimeOffset? LastUpdatedTime { get; set; }

		public DateTimeOffset? DeletedTime { get; set; }

		public bool IsDeleted { get; set; }
	}
}
