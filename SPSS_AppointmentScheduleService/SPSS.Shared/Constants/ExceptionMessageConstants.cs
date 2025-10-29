using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.Constants
{
	public static class ExceptionMessageConstants
	{
		public static class Holiday
		{
			// Validation
			public const string HolidayDataNull = "Holiday data cannot be null.";
			public const string HolidayDateRequired = "Holiday date is required.";
			public const string DescriptionRequired = "Description is required.";
			public const string DescriptionTooLong = "Description cannot exceed 500 characters.";

			// Not found
			public const string NotFound = "Holiday with ID {0} not found.";

			// Business rules
			public const string DuplicateDate = "A holiday already exists for date {0}.";
			public const string CannotModifyPastHoliday = "Cannot modify a holiday date that has already passed.";

			// Transactional
			public const string FailedToCreate = "Failed to create holiday: {0}";
			public const string FailedToUpdate = "Failed to update holiday: {0}";
			public const string FailedToDelete = "Failed to delete holiday: {0}";
		}
	}
}
