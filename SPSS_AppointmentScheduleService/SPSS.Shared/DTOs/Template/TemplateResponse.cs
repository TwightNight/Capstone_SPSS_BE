using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Template
{
	public class TemplateResponse
	{
		public Guid Id { get; set; }

		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		public string Description { get; set; }

		[StringLength(100)]
		public string CreatedBy { get; set; }

		[StringLength(100)]
		public string LastUpdatedBy { get; set; }

		public DateTimeOffset? CreatedTime { get; set; }

		public DateTimeOffset? LastUpdatedTime { get; set; }

		public DateTimeOffset? DeletedTime { get; set; }

		public bool IsDeleted { get; set; }
	}
}
