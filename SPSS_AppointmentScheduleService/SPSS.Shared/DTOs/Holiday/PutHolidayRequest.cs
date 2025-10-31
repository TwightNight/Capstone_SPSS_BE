using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.DTOs.Holiday
{
	public class PutHolidayRequest
	{
		[Column(TypeName = "date")]
		[DataType(DataType.DateTime, ErrorMessage = ExceptionMessageConstants.Holiday.HolidayDateInvalid)]
		public DateTime? HolidayDate { get; set; }

		public string? Description { get; set; }
	}
}
