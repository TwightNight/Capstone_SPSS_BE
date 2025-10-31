using SPSS.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace SPSS.Shared.DTOs.Slot
{
	public class PostSlotRequest
	{
		[Required(ErrorMessage = ExceptionMessageConstants.Slot.SlotMinutesRequired)]
		[Range(1, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Slot.SlotMinutesInvalid)]
		public int SlotMinutes { get; set; }

		[Required(ErrorMessage = ExceptionMessageConstants.Slot.BreakMinutesRequired)]
		[Range(0, int.MaxValue, ErrorMessage = ExceptionMessageConstants.Slot.BreakMinutesInvalid)]
		public int BreakMinutes { get; set; }
	}
}
