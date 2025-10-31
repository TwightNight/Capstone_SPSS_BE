using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Shared.Exceptions
{
	public class BusinessRuleException : Exception
	{
		public BusinessRuleException(string message) : base(message) { }
		public BusinessRuleException(string message, Exception innerException) : base(message, innerException) { }
	}
}
