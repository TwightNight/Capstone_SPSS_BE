using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.DTOs.Holiday
{
	public class PostHolidayRequest
	{
		[Column(TypeName = "date")]
		[Required(ErrorMessage = "Holiday date is required.")]
		public DateTime HolidayDate { get; set; }

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		[Required(ErrorMessage = "Description is required.")]
		public string Description { get; set; }

	}
}
