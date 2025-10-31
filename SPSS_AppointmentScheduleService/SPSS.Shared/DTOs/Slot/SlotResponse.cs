using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Slot
{
	public class SlotResponse
	{
		public Guid Id { get; set; }

		public int SlotMinutes { get; set; }

		public int BreakMinutes { get; set; }

		[StringLength(100)]
		public string? CreatedBy { get; set; }

		[StringLength(100)]
		public string? LastUpdatedBy { get; set; }

		public DateTimeOffset? CreatedTime { get; set; }

		public DateTimeOffset? LastUpdatedTime { get; set; }

		public DateTimeOffset? DeletedTime { get; set; }

		public bool? IsDeleted { get; set; }
	}
}
