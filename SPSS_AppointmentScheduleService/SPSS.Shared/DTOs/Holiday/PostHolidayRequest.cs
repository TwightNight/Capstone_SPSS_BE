using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPSS.Shared.Constants;

namespace SPSS.Shared.DTOs.Holiday
{
	public class PostHolidayRequest
	{
		[Column(TypeName = "date")]
		[Required(ErrorMessage = ExceptionMessageConstants.Holiday.HolidayDateRequired)]
		[DataType(DataType.DateTime, ErrorMessage = ExceptionMessageConstants.Holiday.HolidayDateInvalid)]

		public DateTime HolidayDate { get; set; }

		[StringLength(500, ErrorMessage = ExceptionMessageConstants.Holiday.DescriptionTooLong)]
		[Required(ErrorMessage = ExceptionMessageConstants.Holiday.DescriptionRequired)]
		public string Description { get; set; }

	}
}
