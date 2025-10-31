using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Holiday
{
	public class HolidayResponseDto
	{
		public Guid Id { get; set; }

		public DateTime? HolidayDate { get; set; }

		public string? Description { get; set; }

		public DateTimeOffset? CreatedAt { get; set; }

		public bool? IsDeleted { get; set; }
	}
}
