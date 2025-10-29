using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.DTOs
{
	public class AppointmentResponseDto
	{
		public Guid Id { get; set; }
		public Guid? UserId { get; set; }
		public Guid? StaffId { get; set; }
		public DateTime AppointmentDate { get; set; }
		public DateTimeOffset StartDateTime { get; set; }
		public DateTimeOffset? EndDateTime { get; set; }
		public int? DurationMinutes { get; set; }
		public byte Status { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public DateTimeOffset? UpdatedAt { get; set; }
		public Guid? ServiceId { get; set; }
		public Guid? ScheduleId { get; set; }
		public Guid? SessionId { get; set; }
		public string? Notes { get; set; }
		public bool IsDeleted { get; set; }
	}
}
