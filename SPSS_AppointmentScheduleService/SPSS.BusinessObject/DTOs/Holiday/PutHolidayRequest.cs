﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.BusinessObject.DTOs.Holiday
{
	public class PutHolidayRequest
	{
		public DateTime? HolidayDate { get; set; }

		public string? Description { get; set; }
	}
}
