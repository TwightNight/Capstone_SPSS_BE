using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Slot
{
	public class PutSlotRequest
	{
		[Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Slot.SlotMinutesInvalid)]
		public int SlotMinutes { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Slot.BreakMinutesInvalid)]
		public int BreakMinutes { get; set; }
	}
}
