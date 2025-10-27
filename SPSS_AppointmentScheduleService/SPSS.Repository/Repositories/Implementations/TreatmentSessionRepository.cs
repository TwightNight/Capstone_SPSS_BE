using SPSS.BusinessObject.Context;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Repository.Repositories.Implementations
{
	public class TreatmentSessionRepository : GenericRepository<TreatmentSession, Guid>, ITreatmentSessionRepository
	{
		public TreatmentSessionRepository(SchedulingDBContext context) : base(context)
		{
		}
	}
}
