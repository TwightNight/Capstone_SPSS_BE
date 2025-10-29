using Microsoft.EntityFrameworkCore.Query;
using SPSS.BusinessObject.DTOs.Holiday;
using SPSS.BusinessObject.Models;
using SPSS.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
	public interface IHolidayService
	{
		Task<HolidayResponseDto?> GetByIdAsync(Guid id);
		Task<HolidayResponseDto?> GetByIdAsync(
			Guid id,
			Func<IQueryable<Holiday>, IIncludableQueryable<Holiday, object>>? include = null);
		Task<IEnumerable<HolidayResponseDto>> GetAllAsync();
		Task<IEnumerable<HolidayResponseDto>> GetAsync(
			Expression<Func<Holiday, bool>>? filter = null,
			Func<IQueryable<Holiday>, IOrderedQueryable<Holiday>>? orderBy = null,
			Func<IQueryable<Holiday>, IIncludableQueryable<Holiday, object>>? include = null);
		Task<PagedResponse<HolidayResponseDto>> GetPagedAsync(int pageNumber, int pageSize);
		Task<HolidayResponseDto> CreateAsync(PostHolidayRequest request);
		Task<HolidayResponseDto?> UpdateAsync(Guid id, PutHolidayRequest request);
		Task<bool> DeleteAsync(Guid id);
	}
}
