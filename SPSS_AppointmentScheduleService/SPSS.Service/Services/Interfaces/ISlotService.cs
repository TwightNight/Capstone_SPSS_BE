using Microsoft.EntityFrameworkCore.Query;
using SPSS.BusinessObject.DTOs.Slot;
using SPSS.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface ISlotService
	{
		Task<SlotResponse?> GetByIdAsync(Guid id);
		Task<SlotResponse?> GetByIdAsync(
			Guid id,
			Func<IQueryable<Slot>, IIncludableQueryable<Slot, object>>? include = null);
		Task<IEnumerable<SlotResponse>> GetAllAsync();
		Task<IEnumerable<SlotResponse>> GetAsync(
			Expression<Func<Slot, bool>>? filter = null,
			Func<IQueryable<Slot>, IOrderedQueryable<Slot>>? orderBy = null,
			Func<IQueryable<Slot>, IIncludableQueryable<Slot, object>>? include = null);
	}
}
